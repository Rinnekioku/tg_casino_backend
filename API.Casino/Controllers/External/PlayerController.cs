using API.Casino.Messaging.Handlers;
using API.Casino.Models;
using Microsoft.AspNetCore.Mvc;
using API.Casino.Services.Interfaces;
using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Casino.Controllers.External;

[Route("api/external/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost("Test")]
    public async Task<ActionResult<string>> Test()
    {
        await Task.Yield();
        return Ok("Test");
    }
}