namespace conference_rooms.Domain
{
    public class RoomService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}