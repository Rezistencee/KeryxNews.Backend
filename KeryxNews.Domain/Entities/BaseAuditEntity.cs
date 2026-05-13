namespace KeryxNews.Domain.Entities;

public abstract class BaseAuditEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}