using API.Casino.EventHandlers;
using Common.CasinoServices.Models;
using Microsoft.AspNetCore.Mvc;
using Common.CasinoServices.Services.Interfaces;
using Common.Utils.EventBus.Events;
using Common.Utils.EventBus.Interfaces;

namespace API.Casino.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IEventBus _eventBus;

    public PlayerController(IPlayerService playerService, IEventBus eventBus)
    {
        _playerService = playerService;
        _eventBus = eventBus;

        _eventBus.On<TelegramLogin, TelegramLoginHandler>();
    }

    // Endpoint for increasing player score
    [HttpPost("IncreaseScore")]
    public async Task<ActionResult<Player>> IncreasePlayerScore(string username, int points)
    {
        try
        {
            return Ok(await _playerService.IncreasePlayerScore(username, points));
        }
        catch (Exception)
        {
            return BadRequest($"User with username {username} not found");
        }
    }

    // Endpoint for registering a player
    [HttpPost("Register")]
    public async Task<ActionResult<Player>> RegisterPlayer(string username)
    {
        try
        {
            _eventBus.Publish(new TelegramLogin { Username = "testtest" });
            return Ok(await _playerService.RegisterPlayer(username));
        }
        catch (Exception)
        {
            return BadRequest($"User with username {username} already exists");
        }
    }
}