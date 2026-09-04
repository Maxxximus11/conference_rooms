using conference_rooms.Domain.Interfaces;
using conference_rooms.DTOs.Room;
using conference_rooms.DTOs.RoomService;
using Microsoft.AspNetCore.Mvc;

namespace conference_rooms.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomsController : ControllerBase
    {

        private readonly IRoomRepository _roomRepository;

        public RoomsController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // 1. додавання конференц-залу
        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoomDto createroom)
        {


            var room = new Domain.Room
            {
                Name = createroom.Name,
                Capacity = createroom.Capacity,
                Cost = createroom.Cost,
                Services = createroom.Services.Select(serviceDto => new Domain.RoomService
                {
                    Name = serviceDto.Name,
                    Cost = serviceDto.Cost
                }).ToList()
            };

            await _roomRepository.CreateAsync(room);

            var responseDto = new RoomResponseDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Cost = room.Cost,

                Services = room.Services.Select(service => new RoomServiceDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Cost = service.Cost
                }).ToList()
            };

            return Ok(responseDto);
        }

        // 2. Редагування інформації про зал
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, UpdateRoomDto updateRoomDto)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound($"Зал з ID {id} не знайдено.");
            }

            room.Name = updateRoomDto.Name;
            room.Capacity = updateRoomDto.Capacity;
            room.Cost = updateRoomDto.Cost;

            room.Services = updateRoomDto.Services.Select(serviceDto => new Domain.RoomService
            {
                Name = serviceDto.Name,
                Cost = serviceDto.Cost
            }).ToList();

            await _roomRepository.UpdateAsync(room);

            return Ok(new { Message = "Зал успішно оновлено." });
        }

        // 3. Видалення конференц-залу
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound($"Зал з ID {id} не знайдено.");
            }

            await _roomRepository.DeleteAsync(id);

            return Ok(new { Message = "Зал успішно видалено." });
        }

        // 4. Пошук доступних залів
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime date, [FromQuery] int durationHours, [FromQuery] int capacity)
        {
            var availableRooms = await _roomRepository.GetAvailableRoomsAsync(date, durationHours, capacity);

            var response = availableRooms.Select(room => new RoomResponseDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Cost = room.Cost,
                Services = room.Services.Select(service => new RoomServiceDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Cost = service.Cost
                }).ToList()
            }).ToList();

            return Ok(response);
        }
    }
}
