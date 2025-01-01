using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.ReservationBo
{
    public class DeleteModel : PageModel
    {
        private readonly MvcMovieFrontOffice.Data.ApplicationDbContext _context;

        public DeleteModel(MvcMovieFrontOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ReservationView ReservationView { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationview = await _context.ReservationView.FirstOrDefaultAsync(m => m.ReservationId == id);

            if (reservationview == null)
            {
                return NotFound();
            }
            else
            {
                ReservationView = reservationview;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationview = await _context.ReservationView.FindAsync(id);
            if (reservationview != null)
            {
                ReservationView = reservationview;
                _context.ReservationView.Remove(ReservationView);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
