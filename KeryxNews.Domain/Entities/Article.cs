namespace KeryxNews.Domain.Entities;

public class Article : BaseAuditEntity
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    
    public string Title { get; set; }
    public string Content { get; set; }
    
    public int Views { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public ArticleStatus Status { get; private set; }
    
    private readonly List<Comment> _comments = new List<Comment>();
    public IReadOnlyCollection<Comment> Comments => _comments;
    
    private Article() { }

    public Article(Guid authorId, string title, string content)
    {
        AuthorId = authorId;
        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow;
        Views = 0;
        Status = ArticleStatus.Draft; 
    }
    
    public void SubmitForReview()
    {
        if (Status != ArticleStatus.Draft)
            throw new InvalidOperationException("Only draft can be submitted");

        Status = ArticleStatus.PendingReview;
    }
    
    public void Approve()
    {
        if (Status != ArticleStatus.PendingReview)
            throw new InvalidOperationException("Only pending can be approved");

        Status = ArticleStatus.Approved;
    }

    public void Reject()
    {
        if (Status != ArticleStatus.PendingReview)
            throw new InvalidOperationException("Only pending can be rejected");

        Status = ArticleStatus.Rejected;
    }
    
    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }
}