namespace KeryxNews.Domain.Entities;

public class Article
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    
    public string Title { get; set; }
    public string Content { get; set; }
    
    public int Views { get; set; }
    
    public DateTime CreatedAt { get; set; }

    private Article() { }

    public Article(Guid authorId, string title, string content)
    {
        AuthorId = authorId;
        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}