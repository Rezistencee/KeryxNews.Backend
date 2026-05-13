using KeryxNews.Application.Dtos;
using KeryxNews.Application.Interfaces;

namespace KeryxNews.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IArticleRepository _articleRepository;
    
    public UserService(IUserRepository userRepository, IArticleRepository articleRepository)
    {
        _userRepository = userRepository;
        _articleRepository = articleRepository;
    }

    public async Task<UserWithArticlesDto> GetUserWithArticlesById(Guid id, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByIdAsync(id, ct);
        
        if(user == null)
            throw new Exception("User not found");

        var userArticles = await _articleRepository.GetByUserId(id, ct);
        
        return new UserWithArticlesDto
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            CreatedAt = user.CreatedAt,
            Articles = userArticles.OrderByDescending(a => a.CreatedAt).ToList()
        };
    }
}