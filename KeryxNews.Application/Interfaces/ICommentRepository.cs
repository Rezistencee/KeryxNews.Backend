using KeryxNews.Domain.Entities;

namespace KeryxNews.Application.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken ct = default);
}