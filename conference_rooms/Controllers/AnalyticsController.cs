using conference_rooms.Domain.Interfaces;
using conference_rooms.DTOs.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace conference_rooms.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public AnalyticsController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        /// <summary>
        /// Отримання звіту про доходи від оренди залів.
        /// </summary>
        /// <remarks>
        /// Розраховує та повертає загальну суму доходу, кількість бронювань та загальну тривалість оренди для кожного конференц-залу за вказаний період.
        /// </remarks>
        /// <param name="startDate">Початкова дата для звіту (наприклад, 2026-09-01T00:00:00)</param>
        /// <param name="endDate">Кінцева дата для звіту (наприклад, 2026-09-30T23:59:59)</param>
        /// <returns>Звіт із розрахунком прибутку по кожному залу.</returns>
        /// <response code="200">Звіт успішно згенеровано.</response>
        /// <response code="400">Помилка валідації (якщо початкова дата більша за кінцеву).</response>
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var utcStartDate = startDate.ToUniversalTime();
            var utcEndDate = endDate.ToUniversalTime();

            if (utcStartDate >= utcEndDate)
            {
                return BadRequest("Початкова дата повинна бути меншою за кінцеву.");
            }

            var bookings = await _bookingRepository.GetBookingsByPeriodAsync(utcStartDate, utcEndDate);

            var report = bookings
                .GroupBy(b => b.RoomId)
                .Select(group => new RoomRevenueDto
                {
                    RoomId = group.Key,
                    TotalBookings = group.Count(),
                    TotalDurationHours = group.Sum(b => b.DurationHours),
                    TotalRevenue = group.Sum(b => b.TotalCost)
                })
                .ToList();

            return Ok(report);
        }
    }
}