using Microsoft.AspNetCore.Mvc;

namespace DiscordSecretSanta.Controllers;

public class HealthCheckController : ControllerBase
{
    public const string HealthRoute = "/health";
}