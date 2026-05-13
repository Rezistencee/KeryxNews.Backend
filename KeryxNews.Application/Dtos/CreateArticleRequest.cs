namespace KeryxNews.Dtos;

public sealed class CreateArticleRequest
{
    public string Title { get; init; }
    public string Content { get; init; }
}