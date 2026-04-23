using KeryxNews.Application.Constants;
using Microsoft.AspNetCore.Identity;

namespace KeryxNews.Infrastructure.Persistence;

public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = Roles.All;

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}