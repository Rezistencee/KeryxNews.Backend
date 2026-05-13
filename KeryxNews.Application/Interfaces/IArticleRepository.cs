using KeryxNews.Domain.Entities;

namespace KeryxNews.Application.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Task<IEnumerable<Article>> GetByUserId(Guid userId, CancellationToken ct = default);
    public Task<IEnumerable<Article>> GetTrendingAsync(int count, CancellationToken ct = default);
}