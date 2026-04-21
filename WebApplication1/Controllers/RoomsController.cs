using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        IEnumerable<Room> rooms = InMemoryDataStore.Rooms;

        if (minCapacity.HasValue)
        {
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        }

        if (hasProjector.HasValue)
        {
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        }

        if (activeOnly == true)
        {
            rooms = rooms.Where(r => r.IsActive);
        }

        return Ok(rooms);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetByBuilding(string buildingCode)
    {
        var rooms = InMemoryDataStore.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> Create([FromBody] Room room)
    {
        var newId = InMemoryDataStore.Rooms.Count == 0 ? 1 : InMemoryDataStore.Rooms.Max(r => r.Id) + 1;
        room.Id = newId;
        InMemoryDataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Room> Update(int id, [FromBody] Room updatedRoom)
    {
        var existingRoom = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (existingRoom is null)
        {
            return NotFound();
        }

        updatedRoom.Id = id;

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room is null)
        {
            return NotFound();
        }

        var hasRelatedReservations = InMemoryDataStore.Reservations.Any(res => res.RoomId == id);
        if (hasRelatedReservations)
        {
            return Conflict("Cannot delete room because related reservations exist.");
        }

        InMemoryDataStore.Rooms.Remove(room);
        return NoContent();
    }
}
