using Microsoft.AspNetCore.Identity;

namespace KeryxNews.Infrastructure.Persistence.Models;

public class AppIdentityUser : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
}