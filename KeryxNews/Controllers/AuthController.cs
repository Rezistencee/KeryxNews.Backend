using System.Security.Claims;
using KeryxNews.Application.Constants;
using KeryxNews.Application.Interfaces;
using KeryxNews.Domain.Entities;
using KeryxNews.Dtos;
using KeryxNews.Infrastructure.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KeryxNews.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly SignInManager<AppIdentityUser> _signInManager;

    public AuthController(IAuthService authService, SignInManager<AppIdentityUser> signInManager)
    {
        _authService = authService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto model)
    {
        var userId = await _authService.LoginAsync(model.Email, model.Password);

        if (userId == null)
            return Unauthorized("Invalid email or password");

        return Ok(userId);
    }
    
    [HttpGet("external/google")]
    public IActionResult GoogleLogin()
    {
        string? redirectUrl = Url.Action("GoogleResponse", "Auth", null, Request.Scheme);
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        return Challenge(properties, "Google");
    }

    [HttpGet("external/google/callback")]
    public async Task<IActionResult> GoogleResponse()
    {
        try
        {
            var user = await _authService.AuthenticateWithGoogleAsync();

            if (user == null)
                return BadRequest("Google login failed");

            return Redirect("http://localhost:5173");
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = "Google authentication failed",
                details = ex.Message
            });
        }
    }
        
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
    {
        try
        {
            User newUser = new User(model.Email, model.FullName, string.Empty);
            
            var userId = await _authService.RegisterUserAsync(newUser, model.Password);

            return CreatedAtAction(nameof(Register), new { id = userId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Roles = Roles.User)]
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            return Unauthorized();

        var user = await _authService.GetUserByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        return Ok(new
        {
            user.Id,
            user.FullName,
            user.Email,
            user.AvatarUrl
        });
    }
}