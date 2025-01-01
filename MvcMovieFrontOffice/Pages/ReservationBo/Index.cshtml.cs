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
    public class IndexModel : PageModel
    {
        private readonly MvcMovieFrontOffice.Data.ApplicationDbContext _context;

        public IndexModel(MvcMovieFrontOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ReservationView> ReservationView { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ReservationView = await _context.ReservationView.ToListAsync();
        }
    }
}
