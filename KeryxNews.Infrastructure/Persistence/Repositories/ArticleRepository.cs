using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence.Repositories;

public class ArticleRepository : IRepository<Article>
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

    public async Task<IEnumerable<Article>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Articles.Where(a => a.Status == ArticleStatus.Draft)
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
            .FirstAsync(a => a.Id == article.Id, cancellationToken);

        exists.Title = article.Title;
        exists.Content = article.Content;
        
        _context.Articles.Update(article);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Article article, CancellationToken cancellationToken = default)
    {
        var targetArticle = await GetByIdAsync(article.Id, cancellationToken);
        
        if(targetArticle == null)
            throw new Exception("Article not found");

        _context.Articles.Remove(targetArticle);

        await _context.SaveChangesAsync(cancellationToken);
    }
}