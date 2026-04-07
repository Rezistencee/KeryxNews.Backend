using Microsoft.AspNetCore.Mvc;

namespace KeryxNews.Controllers;

[ApiController]
[Route("initial")]
public class InitialController
{
    [HttpGet]
    public string Get()
    {
        return "Hello";
    }
}