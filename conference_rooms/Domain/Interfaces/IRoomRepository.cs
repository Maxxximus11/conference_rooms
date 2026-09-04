using conference_rooms.Domain;

namespace conference_rooms.Domain.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> CreateAsync(Room room);
        Task<Room?> GetByIdAsync(int id);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);

        Task<List<Room>> GetAvailableRoomsAsync(DateTime startTime, int DurationHours, int capacity);
    }
}