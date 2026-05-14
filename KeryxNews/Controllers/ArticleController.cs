using KeryxNews.Application.Services;
using KeryxNews.Application.Constants;
using KeryxNews.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeryxNews.Controllers;

[ApiController]
[Route("article")]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;

    public ArticleController(ArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        var list = await _articleService.GetAll(page, pageSize, ct);

        return Ok(list);
    }

    [HttpGet("trending")]
    public async Task<IActionResult> GetTrending(CancellationToken ct, [FromQuery] int count = 5)
    {
        var popularArticles = await _articleService.GetTrending(count, ct);

        return Ok(popularArticles);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Author)]
    public async Task<IActionResult> Create(CreateArticleRequest request, CancellationToken ct)
    {
        try
        {
            await _articleService.Add(request, ct);

            return Ok(new { message = "Article created successfully" });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _articleService.GetWithCommentsAsync(id, ct);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Author)]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateArticleRequest request, CancellationToken ct)
    {
        var article = await _articleService.Update(id, request, ct);

        return Ok(new
        {
            article.Id,
            article.Title,
            article.Content
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        try 
        {
            await _articleService.Delete(id, ct);

            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}