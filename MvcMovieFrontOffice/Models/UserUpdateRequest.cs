namespace MvcMovieFrontOffice.Models;

public class UserUpdateRequest
{
    public UserUpdateRequest(string email, string fullName, string role)
    {
        Email = email;
        FullName = fullName;
        Role = role;
    }

    public UserUpdateRequest()
    {
    }

    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
}