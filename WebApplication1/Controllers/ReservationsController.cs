using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        IEnumerable<Reservation> reservations = InMemoryDataStore.Reservations;

        if (date.HasValue)
        {
            reservations = reservations.Where(r => r.Date == date.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        if (roomId.HasValue)
        {
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        }

        return Ok(reservations);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById(int id)
    {
        var reservation = InMemoryDataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> Create([FromBody] Reservation reservation)
    {
        if (reservation.EndTime <= reservation.StartTime)
        {
            return BadRequest("EndTime must be later than StartTime.");
        }

        var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        if (room is null)
        {
            return BadRequest("Room does not exist.");
        }

        if (!room.IsActive)
        {
            return Conflict("Cannot create reservation for an inactive room.");
        }

        var overlaps = InMemoryDataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime);

        if (overlaps)
        {
            return Conflict("Reservation overlaps with an existing reservation.");
        }

        var newId = InMemoryDataStore.Reservations.Count == 0 ? 1 : InMemoryDataStore.Reservations.Max(r => r.Id) + 1;
        reservation.Id = newId;
        InMemoryDataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Update(int id, [FromBody] Reservation updatedReservation)
    {
        var existingReservation = InMemoryDataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (existingReservation is null)
        {
            return NotFound();
        }

        if (updatedReservation.EndTime <= updatedReservation.StartTime)
        {
            return BadRequest("EndTime must be later than StartTime.");
        }

        var room = InMemoryDataStore.Rooms.FirstOrDefault(r => r.Id == updatedReservation.RoomId);
        if (room is null)
        {
            return BadRequest("Room does not exist.");
        }

        if (!room.IsActive)
        {
            return Conflict("Cannot create reservation for an inactive room.");
        }

        var overlaps = InMemoryDataStore.Reservations.Any(r =>
            r.Id != id &&
            r.RoomId == updatedReservation.RoomId &&
            r.Date == updatedReservation.Date &&
            updatedReservation.StartTime < r.EndTime &&
            updatedReservation.EndTime > r.StartTime);

        if (overlaps)
        {
            return Conflict("Reservation overlaps with an existing reservation.");
        }

        updatedReservation.Id = id;

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var reservation = InMemoryDataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation is null)
        {
            return NotFound();
        }

        InMemoryDataStore.Reservations.Remove(reservation);
        return NoContent();
    }
}
