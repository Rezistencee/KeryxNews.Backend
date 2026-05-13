using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly ApiDbContext _context;
    
    public ArticleRepository(ApiDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<Article?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Articles.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Article>> GetAllAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _context.Articles
            .Where(a => a.Status == ArticleStatus.Draft)
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Article article, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(article, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Article article, CancellationToken cancellationToken = default)
    {
        var exists = await _context.Articles
            .FirstOrDefaultAsync(a => a.Id == article.Id, cancellationToken);

        if (exists == null)
            throw new Exception("Article not found");

        exists.Title = article.Title;
        exists.Content = article.Content;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Article article, CancellationToken cancellationToken = default)
    {
        _context.Articles.Remove(article);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Article>> GetByUserId(Guid userId, CancellationToken ct = default)
    {
        return await _context.Articles
            .Where(a => a.AuthorId == userId)
            .ToListAsync(ct);
    }
    
    public async Task<IEnumerable<Article>> GetTrendingAsync(int count, CancellationToken ct = default)
    {
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        
        return await _context.Articles
            .Where(a => a.CreatedAt >= weekAgo)
            .OrderByDescending(a => a.Comments.Count)
            .Take(count)
            .ToListAsync(ct);
    }
}