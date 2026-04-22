using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace KeryxNews.Infrastructure.Persistence;

public static class RoleInjection
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        await RoleSeeder.SeedAsync(roleManager);
    }
}