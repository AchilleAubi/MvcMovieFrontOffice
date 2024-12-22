using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.ViewModels;

public class UpdateProfileViewModel
{
    public UpdateProfileViewModel(string fullName, string? email, string? phoneNumber)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public UpdateProfileViewModel()
    {
    }

    public string FullName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    public string? PhoneNumber { get; set; } 
}