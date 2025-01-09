using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Data;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.ReservationBo
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly MvcMovieFrontOffice.Data.ApplicationDbContext _context;

        public IndexModel(MvcMovieFrontOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ReservationView> ReservationView { get;set; } = default!;
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public string StatusFilter { get; set; }
        public string Keyword { get; set; }
        public DateTime? StartDateFilter { get; set; }
        public DateTime? EndDateFilter { get; set; }

        public async Task OnGetAsync(int pageIndex = 1, int pageSize = 8, string statusFilter = "", string keyword = "", DateTime? startDateFilter = null, DateTime? endDateFilter = null)
        {
            var query = _context.ReservationView.AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
            {
                query = query.Where(r => r.Status.Contains(statusFilter));
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(r => r.FullName.Contains(keyword) || r.Email.Contains(keyword) || r.Model.Contains(keyword) || r.make.Contains(keyword));
            }

            if (startDateFilter.HasValue)
            {
                query = query.Where(r => r.StartDate >= startDateFilter.Value);
            }

            if (endDateFilter.HasValue)
            {
                query = query.Where(r => r.EndDate <= endDateFilter.Value);
            }
            
            var totalReservations = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalReservations / (double)pageSize);
            CurrentPage = pageIndex;
            
            ReservationView = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
