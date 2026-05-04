using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Mappers;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApiDbContext _context;
    
    public UserRepository(ApiDbContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        AppIdentityUser identityUser = await _context.Users.FirstAsync(u => u.Id == id, cancellationToken);

        return identityUser.ToDomain();
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);

        return users.Select(u => u.ToDomain());
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(user.ToEntity(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var targetEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        
        if (targetEntity is null)
            throw new Exception("User not found");
        
        targetEntity.Email = user.Email;
        targetEntity.FullName = user.FullName;
        targetEntity.AvatarUrl = user.AvatarUrl;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async void Delete(User user)
    {
        var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (targetUser is null)
            return;

        _context.Users.Remove(targetUser);

        await _context.SaveChangesAsync();
    }

    public async Task<User> FindByEmail(string email, CancellationToken cancellationToken = default)
    {
        var targetUser = await _context.Users
            .FirstAsync(u => u.Email == email, cancellationToken);
        
        return targetUser.ToDomain();
    }
}