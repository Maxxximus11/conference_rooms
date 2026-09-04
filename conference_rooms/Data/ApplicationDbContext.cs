using Microsoft.EntityFrameworkCore;
using conference_rooms.Domain;

namespace conference_rooms.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomService> RoomServices { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Зал А", Capacity = 50, Cost = 2000m },
                new Room { Id = 2, Name = "Зал В", Capacity = 100, Cost = 3500m },
                new Room { Id = 3, Name = "Зал С", Capacity = 30, Cost = 1500m }
            );

            modelBuilder.Entity<RoomService>().HasData(
                new { Id = 1, Name = "Проєктор", Cost = 500m, RoomId = 1 },
                new { Id = 2, Name = "Wi-Fi", Cost = 300m, RoomId = 1 },
                new { Id = 3, Name = "Звук", Cost = 700m, RoomId = 1 },

                new { Id = 4, Name = "Проєктор", Cost = 500m, RoomId = 2 },
                new { Id = 5, Name = "Wi-Fi", Cost = 300m, RoomId = 2 },
                new { Id = 6, Name = "Звук", Cost = 700m, RoomId = 2 },

                new { Id = 7, Name = "Проєктор", Cost = 500m, RoomId = 3 },
                new { Id = 8, Name = "Wi-Fi", Cost = 300m, RoomId = 3 },
                new { Id = 9, Name = "Звук", Cost = 700m, RoomId = 3 }
            );
        }
    }
}
