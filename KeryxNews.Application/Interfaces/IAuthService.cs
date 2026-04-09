using KeryxNews.Domain.Entities;

namespace KeryxNews.Application.Interfaces;

public interface IAuthService
{
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> AuthenticateWithGoogleAsync();
}