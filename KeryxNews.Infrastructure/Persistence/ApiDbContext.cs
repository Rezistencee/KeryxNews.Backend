using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence;

public class ApiDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>, IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; private set; }
    
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }
}