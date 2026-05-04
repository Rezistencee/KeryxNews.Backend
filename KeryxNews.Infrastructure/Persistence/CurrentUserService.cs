using System.Security.Claims;
using KeryxNews.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KeryxNews.Infrastructure.Persistence;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated != true)
                return null;

            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? user.FindFirst("sub")?.Value;

            return Guid.TryParse(id, out var guid)
                ? guid
                : null;
        }
    }
}