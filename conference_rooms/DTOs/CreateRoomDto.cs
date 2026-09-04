using System.ComponentModel.DataAnnotations;
using conference_rooms.DTOs.RoomService;

namespace conference_rooms.DTOs.Room
{
    public class CreateRoomDto
    {
        [Required(ErrorMessage = "Назва залу є обов'язковою")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 1000, ErrorMessage = "Місткість має бути більше 0")]
        public int Capacity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Вартість оренди не може бути від'ємною")]
        public decimal Cost { get; set; }

        public List<CreateRoomServiceDto> Services { get; set; } = new List<CreateRoomServiceDto>();
    }

    public class UpdateRoomDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Capacity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        public List<CreateRoomServiceDto> Services { get; set; } = new List<CreateRoomServiceDto>();
    }

    public class RoomResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public decimal Cost { get; set; }
        public List<RoomServiceDto> Services { get; set; } = new List<RoomServiceDto>();
    }
}