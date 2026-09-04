using System.ComponentModel.DataAnnotations;
using conference_rooms.DTOs.RoomService;

namespace conference_rooms.DTOs.Booking
{
    public class CreateBookingDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Range(1, 24, ErrorMessage = "Тривалість бронювання має бути від 1 до 24 годин")]
        public int DurationHours { get; set; }

        public List<int> SelectedServiceIds { get; set; } = new List<int>();
    }

    public class BookingResponseDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationHours { get; set; }

        public decimal TotalCost { get; set; }

        public List<RoomServiceDto> SelectedServices { get; set; } = new List<RoomServiceDto>();
    }
}