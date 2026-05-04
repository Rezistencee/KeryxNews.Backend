using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KeryxNews.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUser;

    public AuditInterceptor(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }
    
    private void HandleAudit(DbContext? context)
    {
        if (context == null) return;

        var userId = _currentUser.UserId;
        if (userId is null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is BaseAuditEntity &&
                        (e.State == EntityState.Added ||
                         e.State == EntityState.Modified ||
                         e.State == EntityState.Deleted));

        var logs = new List<UserActivityLog>();

        foreach (var entry in entries)
        {
            var entityType = entry.Entity.GetType().Name;
            var entityId = entry.Property("Id").CurrentValue;

            logs.Add(new UserActivityLog
            {
                UserId = userId.Value,
                ActionType = entry.State switch
                {
                    EntityState.Added => ActionType.Create,
                    EntityState.Modified => ActionType.Update,
                    EntityState.Deleted => ActionType.Delete,
                    _ => ActionType.View
                },
                EntityType = MapEntityType(entityType),
                EntityId = (Guid)entityId,
                CreatedAt = DateTime.UtcNow,
            });
        }

        if (logs.Count > 0)
            context.Set<UserActivityLog>().AddRange(logs);
    }
    
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        HandleAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        HandleAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private EntityType MapEntityType(string name)
    {
        return name switch
        {
            nameof(Article) => EntityType.Article,
            nameof(Comment) => EntityType.Comment,
            nameof(User) => EntityType.User,
            _ => throw new Exception("Unknown entity type")
        };
    }
}