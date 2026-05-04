using KeryxNews.Infrastructure.Persistence.Configuration;
using KeryxNews.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeryxNews.Infrastructure.Persistence;

public static class PersistenceInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        var efSettings = configuration.GetSection("EfCore")
            .Get<EfCoreSettings>();
        
        services.AddDbContext<ApiDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditInterceptor>();
            
            options
                .EnableSensitiveDataLogging(efSettings.EnableSensitiveDataLogging)
                .EnableDetailedErrors(efSettings.EnableDetailedErrors)
                .UseSqlite(connectionString, sqlite =>
                {
                    sqlite.CommandTimeout(efSettings.CommandTimeout);
                })
                .AddInterceptors(interceptor);
        });

        return services;
    }
    
    
}