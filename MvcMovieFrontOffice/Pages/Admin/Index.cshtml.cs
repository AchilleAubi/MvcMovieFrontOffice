using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MvcMovieFrontOffice.Pages.Admin;

[Authorize(Roles = "Admin")]
public class Index : PageModel
{
    public void OnGet()
    {
        
    }
}