using DiscordSecretSanta.Domain.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordSecretSanta.Controllers.Status;

[AllowAnonymous]
public class StatusController : ControllerBase
{
    public const string Route = "/api/setup";

    [HttpGet(Route)]
    public ActionResult<StatusResponse> Get()
    {
        var response = new StatusResponse();

        return Ok(response);
    }
}