using EmulationManager.Server.Services;
using EmulationManager.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EmulationManager.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGames(
        [FromQuery] PlatformType? platform = null,
        [FromQuery] string? search = null)
    {
        var games = await _gameService.GetGamesAsync(platform, search);
        return Ok(games);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGame(int id)
    {
        var game = await _gameService.GetGameDetailAsync(id);
        if (game is null)
            return NotFound();
        return Ok(game);
    }
}
