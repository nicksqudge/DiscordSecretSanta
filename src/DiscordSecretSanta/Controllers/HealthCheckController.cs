using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordSecretSanta.Controllers;

[AllowAnonymous]
public class HealthCheckController : ControllerBase
{
    public const string Route = "/health";
}