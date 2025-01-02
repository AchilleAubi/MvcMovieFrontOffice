using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcMovieFrontOffice.Models;

namespace MvcMovieFrontOffice.Pages.Admin;

public class Edit : PageModel
{
    private readonly UserManager<Users> _userManager;

    public Edit(UserManager<Users> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public Users User { get; set; }

    public string UserRole { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        User = await _userManager.FindByIdAsync(id);
        if (User == null)
        {
            return NotFound();
        }
        
        var roles = await _userManager.GetRolesAsync(User);
        UserRole = roles.FirstOrDefault();

        return Page();
    }
}