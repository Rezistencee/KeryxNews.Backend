using KeryxNews.Application.Interfaces;
using KeryxNews.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KeryxNews.Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;

    public UserController(IUserRepository userRepository, UserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _userRepository.GetByIdAsync(id, ct);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    
    [HttpGet("{id}/articles")]
    public async Task<IActionResult> GetWithArticlesById([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            var user = await _userService.GetUserWithArticlesById(id, ct);

            return Ok(user);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}