using Microsoft.AspNetCore.Identity;
using RealEstate.Entity.Concrete;

namespace RealEstate.API.Tools;

public static class SeedData
{
    // Bu metot program başlarken çalışacak
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        // 1. "Admin" Rolü var mı? Yoksa oluştur.
        string[] roleNames = { "Admin", "Agent", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole { Name = roleName });
            }
        }

        // 2. "Admin" Kullanıcısı var mı? Yoksa oluştur.
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                FirstName = "System",
                LastName = "Admin",
                UserName = "admin",
                Email = "admin@realestate.com",
                IsAgent = false,
                EmailConfirmed = true
            };

            // Şifresi: Admin123!
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            
            if (result.Succeeded)
            {
                // Ona "Admin" rolünü ver
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}