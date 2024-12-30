using Microsoft.AspNetCore.Identity;

namespace MvcMovieFrontOffice.Models;

public static class DataSeeder
{
    public static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Users>>();
        
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("Client"))
        {
            await roleManager.CreateAsync(new IdentityRole("Client"));
        }
       
        var users = new Users
        {
            FullName = "Admin",
            Email = "admin@admin.com",
            UserName = "admin@admin.com",
        };
        
        var adminPassword = "Tsikah2006@!";
        var adminUser = await userManager.FindByEmailAsync(users.Email);
        if (adminUser == null)
        {
            var result = await userManager.CreateAsync(users, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(users, "Admin");
            }
        }
    }
}