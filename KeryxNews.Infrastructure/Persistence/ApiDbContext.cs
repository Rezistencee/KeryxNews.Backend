using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence;

public class ApiDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>, IDataProtectionKeyContext
{
    public DbSet<Article> Articles { get; private set; }
    public DbSet<Comment> Comments { get; private set; }
    public DbSet<UserActivityLog> UserActivityLogs { get; set; }
    
    public DbSet<DataProtectionKey> DataProtectionKeys { get; private set; }
     
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
    }
}