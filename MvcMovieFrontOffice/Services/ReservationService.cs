using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Services;

public class ReservationService(ApplicationDbContext context)
{
    private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations.ToListAsync();
    }
    
    public async Task<Reservation?> GetReservationByIdAsync(int id)
    {
        return await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task CreateReservationAsync(Reservation reservation)
    {
        _context.Add(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateReservationAsync(Reservation reservation)
    {
        _context.Update(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }

    public bool ReservationExists(int id)
    {
        return _context.Reservations.Any(r => r.Id == id);
    }
}