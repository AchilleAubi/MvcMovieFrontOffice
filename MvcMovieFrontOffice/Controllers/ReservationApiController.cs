using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.Services;

namespace MvcMovieFrontOffice.Controllers;

[Route("api/reservations")]
[ApiController]
[Authorize]
public class ReservationApiController(ReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
    {
        var reservations = await reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservation(int id)
    {
        var reservation = await reservationService.GetReservationByIdAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await reservationService.CreateReservationAsync(reservation);
        return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
    {
        if (id != reservation.Id)
        {
            return BadRequest("ID mismatch.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await reservationService.UpdateReservationAsync(reservation);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!reservationService.ReservationExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservation(int id)
    {
        if (!reservationService.ReservationExists(id))
        {
            return NotFound();
        }

        await reservationService.DeleteReservationAsync(id);
        return NoContent();
    }
}
