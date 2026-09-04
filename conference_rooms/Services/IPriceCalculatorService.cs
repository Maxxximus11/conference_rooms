using conference_rooms.Domain;

namespace conference_rooms.Services
{
    public interface IPriceCalculatorService
    {
        decimal CalculateTotalCost(Room room, DateTime startTime, int durationHours, List<RoomService> selectedServices);
    }
}