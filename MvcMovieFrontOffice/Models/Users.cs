using Microsoft.AspNetCore.Identity;

namespace MvcMovieFrontOffice.Models;

public class Users : IdentityUser
{
    public string FullName { get; set; }
}