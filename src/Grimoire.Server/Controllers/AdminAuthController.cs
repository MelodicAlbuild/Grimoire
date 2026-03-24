using Grimoire.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Grimoire.Server.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminAuthController : ControllerBase
{
    private readonly IAdminAuthService _auth;
    public const string CookieName = "GrimoireAdmin";

    public AdminAuthController(IAdminAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public IActionResult Login([FromForm] string key, [FromForm] string? returnUrl)
    {
        if (!_auth.ValidateKey(key))
        {
            return Redirect($"/admin?error=invalid&returnUrl={Uri.EscapeDataString(returnUrl ?? "/admin/games")}");
        }

        Response.Cookies.Append(CookieName, "authenticated", new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            MaxAge = TimeSpan.FromHours(12),
            IsEssential = true
        });

        return Redirect(returnUrl ?? "/admin/games");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(CookieName);
        return Redirect("/");
    }
}
