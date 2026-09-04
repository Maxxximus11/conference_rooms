using System.ComponentModel.DataAnnotations;

namespace conference_rooms.DTOs.RoomService
{
    public class CreateRoomServiceDto
    {
        [Required(ErrorMessage = "Назва послуги є обов'язковою")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Вартість не може бути від'ємною")]
        public decimal Cost { get; set; }
    }

    public class RoomServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
    }
}
