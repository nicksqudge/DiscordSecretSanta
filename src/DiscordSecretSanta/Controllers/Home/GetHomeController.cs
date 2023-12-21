using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscordSecretSanta.Controllers.Home;

[AllowAnonymous]
public class GetHomeController : Controller
{
    public const string Route = "/api/home";
}