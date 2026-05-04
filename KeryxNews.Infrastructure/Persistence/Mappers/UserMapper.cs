using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Models;

namespace KeryxNews.Infrastructure.Persistence.Mappers;

public static class UserMapper
{
    public static User ToDomain(AppIdentityUser entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        return new User(
            entity.Id,
            entity.Email,
            entity.FullName,
            entity.AvatarUrl
        );
    }

    public static AppIdentityUser ToEntity(User domain)
    {
        if (domain is null)
            throw new ArgumentNullException(nameof(domain));

        return new AppIdentityUser
        {
            Id = domain.Id,
            Email = domain.Email,
            FullName = domain.FullName,
            AvatarUrl = domain.AvatarUrl
        };
    }
}

public static class UserMappingExtensions
{
    public static User ToDomain(this AppIdentityUser entity)
        => UserMapper.ToDomain(entity);

    public static AppIdentityUser ToEntity(this User user)
        => UserMapper.ToEntity(user);
}