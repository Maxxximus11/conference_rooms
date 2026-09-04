using conference_rooms.Domain.Interfaces;
using conference_rooms.DTOs.Booking;
using conference_rooms.DTOs.RoomService;
using conference_rooms.Services;
using Microsoft.AspNetCore.Mvc;

namespace conference_rooms.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IPriceCalculatorService _priceCalculatorService;

        public BookingsController(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IPriceCalculatorService priceCalculatorService)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _priceCalculatorService = priceCalculatorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto bookingDto)
        {
            var room = await _roomRepository.GetByIdAsync(bookingDto.RoomId);
            if (room == null)
            {
                return NotFound($"Зал з ID {bookingDto.RoomId} не знайдено.");
            }

            bool isOverlap = await _bookingRepository.HasOverlappingBookingsAsync(
                bookingDto.RoomId,
                bookingDto.StartTime,
                bookingDto.DurationHours);

            if (isOverlap)
            {
                return Conflict("Цей зал вже заброньовано на обраний час. Будь ласка, оберіть інші години.");
            }

            var selectedServices = room.Services
                .Where(service => bookingDto.SelectedServiceIds.Contains(service.Id))
                .ToList();

            decimal servicesTotalCost = selectedServices.Sum(s => s.Cost);

            decimal totalCost = 0;

            try
            {
                totalCost = _priceCalculatorService.CalculateTotalCost(
                room,
                bookingDto.StartTime,
                bookingDto.DurationHours,
                selectedServices
            );
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            var booking = new Domain.Booking
            {
                RoomId = bookingDto.RoomId,
                StartTime = bookingDto.StartTime,
                DurationHours = bookingDto.DurationHours,
                TotalCost = totalCost,
                SelectedServices = selectedServices
            };

            var savedBooking = await _bookingRepository.CreateAsync(booking);

            var response = new BookingResponseDto
            {
                Id = savedBooking.Id,
                RoomId = savedBooking.RoomId,
                StartTime = savedBooking.StartTime,
                DurationHours = savedBooking.DurationHours,
                TotalCost = savedBooking.TotalCost,
                SelectedServices = savedBooking.SelectedServices.Select(s => new RoomServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cost = s.Cost
                }).ToList()
            };

            return Ok(response);
        }
    }
}