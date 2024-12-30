using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcMovieFrontOffice.Models;
using MvcMovieFrontOffice.ViewModels;

namespace MvcMovieFrontOffice.Controllers;

public class AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager)
    : Controller
{
    public IActionResult Login(string returnUrl)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        if (ModelState.IsValid)
        {
            
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        return View(model);
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var users = new Users
        {
            FullName = model.Name,
            Email = model.Email,
            UserName = model.Email
        };
            
        var result = await userManager.CreateAsync(users, model.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(users, "Client");
            return RedirectToAction("Login", "Account");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }   
            return View(model);
        }
    }
    
    public IActionResult VerifyEmail()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email address.");
                return View(model);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
            }
        }
        return View(model);
    }
    
    public IActionResult ChangePassword(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("VerifyEmail", "Account");
        }
        return View(new ChangePasswordViewModel { Email = username });
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user != null)
            {
                var result = await userManager.RemovePasswordAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, model.NewPassword);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }   
                    return View(model); 
                }
            }
            else
            {
                ModelState.AddModelError("", "Email not found.");
                return View(model);
            }
        }
        else
        {
            ModelState.AddModelError("", "Something went wrong. try again.");
            return View(model); 
        }
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Vehicle");
    }
    
    [HttpPost("api/login")]
    public async Task<IActionResult> LoginApi([FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid data provided." });
        }

        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Invalid login attempt." });
        }
        
        return Ok(new { message = "Login successful" });
    }
    
    public IActionResult UpdateProfile()
    {
        var user = userManager.GetUserAsync(User).Result;

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var model = new UpdateProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        return View(model);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            ModelState.AddModelError("", "User not found.");
            return View(model);
        }
        
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.UserName = model.Email;
        user.PhoneNumber = model.PhoneNumber;

        var result = await userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            //return RedirectToAction("", "Vehicle");
            return View(new UpdateProfileViewModel { FullName = user.FullName, Email = user.Email });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }
}