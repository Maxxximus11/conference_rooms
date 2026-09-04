using Microsoft.EntityFrameworkCore;
using conference_rooms.Data;
using conference_rooms.Domain;
using conference_rooms.Domain.Interfaces;

namespace conference_rooms.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> HasOverlappingBookingsAsync(int roomId, DateTime startTime, int durationHours)
        {
            DateTime endTime = startTime.AddHours(durationHours);

            return await _context.Bookings
                .AnyAsync(b => b.RoomId == roomId &&
                               startTime < b.StartTime.AddHours(b.DurationHours) &&
                               endTime > b.StartTime);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Bookings
                .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
                .ToListAsync();
        }
    }
}
