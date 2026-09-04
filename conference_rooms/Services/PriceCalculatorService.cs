using conference_rooms.Domain;

namespace conference_rooms.Services
{
    public class PriceCalculatorService : IPriceCalculatorService
    {
        public decimal CalculateTotalCost(Room room, DateTime startTime, int durationHours, List<RoomService> selectedServices)
        {
            decimal totalCost = 0;

            decimal costPerMinute = room.Cost / 60m;
            int totalMinutes = durationHours * 60;

            for (int i = 0; i < totalMinutes; i++)
            {
                var currentMinute = startTime.AddMinutes(i);
                var hour = currentMinute.Hour;

                decimal currentMinuteRate = costPerMinute;

                if (hour >= 6 && hour < 9)
                {
                    currentMinuteRate *= 0.9m; // -10%
                }
                else if (hour >= 12 && hour < 14)
                {
                    currentMinuteRate *= 1.15m; // +15%
                }
                else if (hour >= 18 && hour < 23)
                {
                    currentMinuteRate *= 0.8m; // -20%
                }

                totalCost += currentMinuteRate;
            }

            if (selectedServices != null && selectedServices.Any())
            {
                totalCost += selectedServices.Sum(s => s.Cost);
            }

            return Math.Round(totalCost, 2);
        }
    }
}