using KeryxNews.Domain.Entities;

namespace KeryxNews.Infrastructure.Persistence.Models;

public class UserActivityLog
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    
    public ActionType ActionType { get; set; }
    public EntityType EntityType { get; set; }
    public Guid EntityId { get; set; }

    public DateTime CreatedAt { get; set; }
}