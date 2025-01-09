using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MvcMovieFrontOffice.Pages.Admin;

[Authorize(Roles = "Admin")]
public class Create : PageModel
{
    public void OnGet()
    {
        
    }
}