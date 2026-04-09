using System.Security.Claims;
using KeryxNews.Application.Interfaces;
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

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Auth", null, Request.Scheme);
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        return Challenge(properties, "Google");
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var user = await _authService.AuthenticateWithGoogleAsync();

        if (user == null)
            return BadRequest("Google login failed");

        return Ok();
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (userIdClaim == null) 
            return Unauthorized();

        var user = await _authService.GetUserByIdAsync(Guid.Parse(userIdClaim));
        
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