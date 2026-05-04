namespace KeryxNews.Domain.Entities;

public class Comment : BaseAuditEntity
{
    public Guid Id { get; private set; }
    public string Content { get; private set; }

    public Guid ArticleId { get; private set; }
    public Guid AuthorId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    private Comment() { }

    public Comment(Guid articleId, Guid authorId, string content)
    {
        Id = Guid.NewGuid();
        ArticleId = articleId;
        AuthorId = authorId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}