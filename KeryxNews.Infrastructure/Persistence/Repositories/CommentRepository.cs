using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApiDbContext _context;
    
    public CommentRepository(ApiDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Comments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Comment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Comments.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteAsync(Guid commentId, CancellationToken cancellationToken = default)
    {
        var targetComment = await GetByIdAsync(commentId, cancellationToken);

        if (targetComment == null)
            return false;

        _context.Comments.Remove(targetComment);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IEnumerable<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken ct = default)
    {
        return await _context.Comments
            .Where(c => c.ArticleId == articleId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(ct);
    }
}