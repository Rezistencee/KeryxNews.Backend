namespace KeryxNews.Application;

public class ArticleWithCommentsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public int Views { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<CommentDto> Comments { get; set; } = new();
}

public class CommentDto
{
    public string Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}