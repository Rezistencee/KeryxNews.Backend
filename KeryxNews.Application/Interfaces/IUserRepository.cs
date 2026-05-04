using KeryxNews.Domain.Entities;

namespace KeryxNews.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> FindByEmail(string email, CancellationToken ct = default);
}