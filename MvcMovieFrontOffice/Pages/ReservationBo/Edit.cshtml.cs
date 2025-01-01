using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.ReservationBo
{
    public class EditModel : PageModel
    {
        private readonly MvcMovieFrontOffice.Data.ApplicationDbContext _context;

        public EditModel(MvcMovieFrontOffice.Data.ApplicationDbContext context)
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

            var reservationview =  await _context.ReservationView.FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservationview == null)
            {
                return NotFound();
            }
            ReservationView = reservationview;
            return Page();
        }

        private bool ReservationViewExists(int id)
        {
            return _context.ReservationView.Any(e => e.ReservationId == id);
        }
    }
}
