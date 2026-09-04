using Microsoft.EntityFrameworkCore;
using conference_rooms.Data;
using conference_rooms.Domain;
using conference_rooms.Domain.Interfaces;

namespace conference_rooms.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room> CreateAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Services)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime startTime, int DurationHours, int capacity)
        {
            var endTime = startTime.AddHours(DurationHours);

            var availableRooms = await _context.Rooms
                .Where(room => room.Capacity >= capacity)
                .Where(room => !_context.Bookings.Any(booking =>
                    booking.RoomId == room.Id &&
                    startTime < booking.StartTime.AddHours(booking.DurationHours) &&
                    endTime > booking.StartTime))
                .ToListAsync();

            return availableRooms;
        }

    }
}
