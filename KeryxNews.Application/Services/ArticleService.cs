using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using KeryxNews.Dtos;

namespace KeryxNews.Application.Services;

public class ArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUserService _currentUserService;

    public ArticleService(IArticleRepository articleRepository, ICommentRepository commentRepository, 
        ICurrentUserService currentUserService)
    {
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
        _currentUserService = currentUserService;
    }
    
    public async Task<IEnumerable<Article>> GetAll(
        int page = 1,
        int pageSize = 10,
        CancellationToken ct = default)
    {
        return await _articleRepository.GetAllAsync(page, pageSize, ct);
    }

    public async Task<Article> GetById(Guid articleId, CancellationToken ct = default)
    {
        var article = await _articleRepository.GetByIdAsync(articleId, ct);

        if (article == null)
            throw new Exception("Article not found");

        return article;
    }
    
    public async Task Add(CreateArticleRequest request, CancellationToken ct = default)
    {
        var userId = _currentUserService.UserId;
    
        if (userId == null)
            throw new UnauthorizedAccessException();
    
        var article = new Article(userId.Value, request.Title, request.Content);
    
        await _articleRepository.AddAsync(article, ct);
    }
    
    public async Task<Article> Update(Guid id, UpdateArticleRequest request, CancellationToken ct = default)
    {
        var userId = _currentUserService.UserId;

        if (userId == null)
            throw new UnauthorizedAccessException();

        var article = await _articleRepository.GetByIdAsync(id, ct);

        if (article == null)
            throw new Exception("Article not found");

        if (article.AuthorId != userId.Value)
            throw new Exception("No rights");

        article.Title = request.Title;
        article.Content = request.Content;

        await _articleRepository.UpdateAsync(article, ct);

        return article;
    }

    public async Task Delete(Guid id, CancellationToken ct = default)
    {
        var article = await _articleRepository.GetByIdAsync(id, ct);

        if (article == null)
            throw new Exception("Article not found");

        await _articleRepository.DeleteAsync(article, ct);
    }
    
    public async Task<ArticleWithCommentsDto?> GetWithCommentsAsync(Guid articleId, CancellationToken ct = default)
    {
        var article = await _articleRepository.GetByIdAsync(articleId, ct);
        
        if (article == null)
            return null;
        
        var comments = await _commentRepository.GetByArticleIdAsync(articleId, ct);
        
        return new ArticleWithCommentsDto
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            ImageUrl = article.ImageUrl,
            Views = article.Views,
            CreatedAt = article.CreatedAt,
            Comments = comments.Select(c => new CommentDto
            {
                Content = c.Content,
                CreatedAt = c.CreatedAt
            }).ToList()
        };
    }
    
    public async Task<IEnumerable<Article>> GetTrending(int count = 5, CancellationToken ct = default)
    {
        return await _articleRepository.GetTrendingAsync(count, ct);
    }
}