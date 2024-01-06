using DiscordSecretSanta.Domain.Setup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace DiscordSecretSanta.Controllers.Home;

[AllowAnonymous]
public class GetHomeController : Controller
{
    public const string Route = "/api/home";

    [HttpGet]
    public async Task<ActionResult<SetupResponse>> Get()
    {
        return Ok(new SetupResponse());
    }
}