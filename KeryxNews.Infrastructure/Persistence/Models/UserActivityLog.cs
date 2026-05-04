namespace KeryxNews.Infrastructure.Persistence.Models;

public class UserActivityLog
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public int ActionId { get; set; }

    public int EntityType { get; set; }
    public Guid EntityId { get; set; }

    public DateTime CreatedAt { get; set; }
}