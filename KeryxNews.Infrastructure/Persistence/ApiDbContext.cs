using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence;

public class ApiDbContext : IdentityDbContext<AppIdentityUser, IdentityRole<Guid>, Guid>
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }
}