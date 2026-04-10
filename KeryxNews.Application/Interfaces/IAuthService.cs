using KeryxNews.Domain.Entities;

namespace KeryxNews.Application.Interfaces;

public interface IAuthService
{
    Task<Guid?> AuthenticateWithGoogleAsync();
    Task<Guid> RegisterUserAsync(User user, string password);
    Task<Guid?> LoginAsync(string email, string passwword);
    Task<User> GetUserByIdAsync(Guid id);
    Task<Guid?> GetUserByEmailAsync(string email);
}