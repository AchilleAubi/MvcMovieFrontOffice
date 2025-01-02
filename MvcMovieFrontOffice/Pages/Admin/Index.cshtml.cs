using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.Admin;

[Authorize(Roles = "Admin")]
public class Index : PageModel
{
    private readonly UserManager<Users> _userManager;

    public Index(UserManager<Users> userManager)
    {
        _userManager = userManager;
    }
    
    public List<Users> UsersList { get; set; }
    
    public async Task OnGetAsync()
    {
        UsersList = await _userManager.Users.ToListAsync();
    }
}