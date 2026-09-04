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

        // 1. Додавання конференц-залу

        /// <summary>
        /// Додавання нового конференц-залу.
        /// </summary>
        /// <remarks>
        /// Створює новий зал із вказаною назвою, місткістю, базовою вартістю оренди та списком доступних послуг.
        /// </remarks>
        /// <param name="createroom">Об'єкт з даними для створення нового залу</param>
        /// <returns>Підтвердження успішного створення залу з детальною інформацією.</returns>
        /// <response code="200">Зал успішно створено.</response>
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

        /// <summary>
        /// Редагування інформації про існуючий зал.
        /// </summary>
        /// <remarks>
        /// Оновлює назву, місткість, вартість та список послуг для залу за його унікальним ідентифікатором.
        /// </remarks>
        /// <param name="id">Унікальний ідентифікатор залу, який потрібно оновити</param>
        /// <param name="updateRoomDto">Нові дані для залу</param>
        /// <returns>Підтвердження успішного оновлення.</returns>
        /// <response code="200">Зал успішно оновлено.</response>
        /// <response code="404">Зал з вказаним ID не знайдено.</response>
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

        /// <summary>
        /// Видалення конференц-залу.
        /// </summary>
        /// <remarks>
        /// Безповоротно видаляє зал та всі пов'язані з ним послуги з бази даних.
        /// </remarks>
        /// <param name="id">Унікальний ідентифікатор залу для видалення</param>
        /// <returns>Підтвердження видалення.</returns>
        /// <response code="200">Зал успішно видалено.</response>
        /// <response code="404">Зал з вказаним ID не знайдено.</response>
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

        /// <summary>
        /// Пошук доступних конференц-залів.
        /// </summary>
        /// <remarks>
        /// Повертає список залів, які не заброньовані на вказаний час та мають достатню місткість.
        /// </remarks>
        /// <param name="date">Дата початку оренди (наприклад, 2026-09-01T10:00:00)</param>
        /// <param name="durationHours">Тривалість оренди в годинах</param>
        /// <param name="capacity">Мінімальна кількість осіб, яку має вміщувати зал</param>
        /// <returns>Список доступних залів.</returns>
        /// <response code="200">Пошук виконано успішно.</response>
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