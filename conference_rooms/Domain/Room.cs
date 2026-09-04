namespace conference_rooms.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal Cost { get; set; }
        public List<RoomService> Services { get; set; } = new List<RoomService>();
    }
}