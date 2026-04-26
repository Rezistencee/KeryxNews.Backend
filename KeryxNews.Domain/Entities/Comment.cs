namespace KeryxNews.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    
    public Guid ArticleId { get; set; }
    public Guid AuthorId { get; set; }
    
    public DateTime CreatedAt { get; set; }

    private Comment() { }
}