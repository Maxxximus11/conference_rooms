namespace conference_rooms.DTOs.Analytics
{
    public class RoomRevenueDto
    {
        public int RoomId { get; set; }
        public int TotalBookings { get; set; }
        public int TotalDurationHours { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}