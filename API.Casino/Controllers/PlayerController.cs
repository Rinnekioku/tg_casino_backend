using API.Casino.Messaging.Handlers;
using Common.CasinoServices.Models;
using Microsoft.AspNetCore.Mvc;
using Common.CasinoServices.Services.Interfaces;
using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Casino.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    // Endpoint for increasing player score
    [HttpPost("IncreaseScore")]
    [Authorize]
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
            return Ok(await _playerService.RegisterPlayer(username));
        }
        catch (Exception)
        {
            return BadRequest($"User with username {username} already exists");
        }
    }
}