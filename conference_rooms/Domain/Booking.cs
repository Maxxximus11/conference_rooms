using System;
using System.Collections.Generic;

namespace conference_rooms.Domain
{
    public class Booking
    {
        public int Id { get; set; }

        public int RoomId { get; set; }
        public Room? Room { get; set; }

        public DateTime StartTime { get; set; }
        public int DurationHours { get; set; }
        public decimal TotalCost { get; set; }

        public List<RoomService> SelectedServices { get; set; } = new List<RoomService>();
    }
}