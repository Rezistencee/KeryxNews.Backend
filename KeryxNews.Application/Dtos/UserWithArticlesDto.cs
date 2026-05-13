using KeryxNews.Domain.Entities;

namespace KeryxNews.Application.Dtos;

public class UserWithArticlesDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Article> Articles { get; set; } = new();
}