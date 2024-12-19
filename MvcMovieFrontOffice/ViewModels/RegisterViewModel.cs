using System.ComponentModel.DataAnnotations;

namespace MvcMovieFrontOffice.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Please enter your Name")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Please enter your Email")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Please enter your Password")]
    [StringLength(40, ErrorMessage = "Password must be between 8 and 40 characters", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Compare("ConfirmPassword", ErrorMessage = "Passwords don't match")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Please confirm your Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
    
    
}