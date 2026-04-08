using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeryxNews.Infrastructure.Persistence;

public static class PersistenceInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var efSettings = configuration.GetSection("EfCore");
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApiDbContext>(options =>
        {
            options
                .EnableSensitiveDataLogging(efSettings.GetValue<bool>("EnableSensitiveDataLogging"))
                .EnableDetailedErrors(efSettings.GetValue<bool>("EnableDetailedErrors"))
                .UseSqlite(connectionString, sqlite =>
                {
                    sqlite.CommandTimeout(efSettings.GetValue<int>("CommandTimeout"));
                });
        });

        return services;
    }
}