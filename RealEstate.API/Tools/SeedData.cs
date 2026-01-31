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

        // 1. Rolleri oluştur
        string[] roleNames = { "Admin", "Agent", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new AppRole { Name = roleName });
            }
        }

        // 2. İstenen Kullanıcıları Oluştur
        // Admin
        if (await userManager.FindByEmailAsync("admin@test.com") == null)
        {
            var admin = new AppUser { UserName = "admin", Email = "admin@test.com", FirstName = "Admin", LastName = "User", IsAgent = false, EmailConfirmed = true };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Agent (Emlakçı)
        if (await userManager.FindByEmailAsync("agent@test.com") == null)
        {
            var agent = new AppUser { UserName = "agent", Email = "agent@test.com", FirstName = "Agent", LastName = "User", IsAgent = true, EmailConfirmed = true };
            await userManager.CreateAsync(agent, "Agent123!");
            await userManager.AddToRoleAsync(agent, "Agent");
        }

        // User (Normal Müşteri)
        if (await userManager.FindByEmailAsync("user@test.com") == null)
        {
            var user = new AppUser { UserName = "user", Email = "user@test.com", FirstName = "Normal", LastName = "User", IsAgent = false, EmailConfirmed = true };
            await userManager.CreateAsync(user, "User123!");
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}