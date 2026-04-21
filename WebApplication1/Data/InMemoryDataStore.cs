using WebApplication1.Models;

namespace WebApplication1.Data;

public static class InMemoryDataStore
{
    public static List<Room> Rooms { get; } =
    [
        new Room { Id = 1, Name = "Room 101", BuildingCode = "A", Floor = 1, Capacity = 20, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = true },
        new Room { Id = 3, Name = "Room 305", BuildingCode = "A", Floor = 3, Capacity = 12, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Conference 12", BuildingCode = "C", Floor = 1, Capacity = 40, HasProjector = true, IsActive = false },
        new Room { Id = 5, Name = "Room 210", BuildingCode = "B", Floor = 2, Capacity = 18, HasProjector = false, IsActive = true }
    ];

    public static List<Reservation> Reservations { get; } =
    [
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Anna Kowalska", Topic = "C# Basics", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(9, 0, 0), EndTime = new TimeOnly(10, 30, 0), Status = "planned" },
        new Reservation { Id = 2, RoomId = 2, OrganizerName = "Marek Nowak", Topic = "HTTP and REST Workshop", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(10, 0, 0), EndTime = new TimeOnly(12, 30, 0), Status = "confirmed" },
        new Reservation { Id = 3, RoomId = 3, OrganizerName = "Ewa Zielinska", Topic = "Postman Tests", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(8, 30, 0), EndTime = new TimeOnly(10, 0, 0), Status = "planned" },
        new Reservation { Id = 4, RoomId = 5, OrganizerName = "Piotr Wisniewski", Topic = "Routing Practice", Date = new DateOnly(2026, 5, 12), StartTime = new TimeOnly(11, 0, 0), EndTime = new TimeOnly(12, 0, 0), Status = "cancelled" },
        new Reservation { Id = 5, RoomId = 1, OrganizerName = "Katarzyna Lewandowska", Topic = "Model Binding", Date = new DateOnly(2026, 5, 13), StartTime = new TimeOnly(13, 0, 0), EndTime = new TimeOnly(14, 30, 0), Status = "confirmed" },
        new Reservation { Id = 6, RoomId = 2, OrganizerName = "Tomasz Wozniak", Topic = "Validation Workshop", Date = new DateOnly(2026, 5, 14), StartTime = new TimeOnly(9, 30, 0), EndTime = new TimeOnly(11, 0, 0), Status = "planned" }
    ];
}
