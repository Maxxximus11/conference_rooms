using conference_rooms.Domain;

namespace conference_rooms.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateAsync(Booking booking);

        Task<bool> HasOverlappingBookingsAsync(int roomId, DateTime startTime, int durationHours);

        Task<IEnumerable<Domain.Booking>> GetBookingsByPeriodAsync(DateTime startDate, DateTime endDate);
    }
}