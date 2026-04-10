using System.Security.Claims;
using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KeryxNews.Infrastructure.Persistence;

public class AuthService : IAuthService
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager;

    public AuthService(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        
        if (appUser == null) 
            return null;

        return new User(appUser.Id, appUser.Email, appUser.FullName, appUser.AvatarUrl);
    }

    public async Task<Guid?> GetUserByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email);
        
        if (appUser == null) 
            return null;

        return appUser.Id;
    }
    
    public async Task<Guid?> LoginAsync(string email, string password)
    {
        var appUser = await _userManager.FindByEmailAsync(email);

        if (appUser == null)
            return null;

        var result = await _signInManager.PasswordSignInAsync(appUser, password, true, false);

        if (!result.Succeeded)
            return null;

        return appUser.Id;
    }
    
    
    public async Task<Guid?> AuthenticateWithGoogleAsync()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        
        if (info == null) 
            return null;

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);

        if (signInResult.Succeeded)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var appUser = await _userManager.FindByEmailAsync(email);

            return appUser.Id;
        }

        var newUser = new AppIdentityUser
        {
            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
            FullName = info.Principal.FindFirstValue(ClaimTypes.Name),
            AvatarUrl = info.Principal.FindFirstValue("image")
        };

        await _userManager.CreateAsync(newUser);
        await _userManager.AddLoginAsync(newUser, info);
        await _signInManager.SignInAsync(newUser, true);

        return newUser.Id;
    }

    public async Task<Guid> RegisterUserAsync(User user, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(user.Email);

        if (existingUser != null)
            throw new InvalidOperationException("User already exists");
        
        var newUser = new AppIdentityUser
        {
            Email = user.Email,
            UserName = user.Email,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl
        };

        var result = await _userManager.CreateAsync(newUser, password);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _signInManager.SignInAsync(newUser, false);
        
        return newUser.Id;
    }
}