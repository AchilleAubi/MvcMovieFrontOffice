using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.ViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Please enter your Password")]
    [StringLength(40, ErrorMessage = "Password must be between 8 and 40 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    [Compare("ConfirmNewPassword", ErrorMessage = "Passwords don't match")]
    public string NewPassword { get; set; }
    
    [Required(ErrorMessage = "Please confirm your Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    public string ConfirmNewPassword { get; set; }
}