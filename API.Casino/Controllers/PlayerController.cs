using API.Casino.Messaging.Handlers;
using API.Casino.Models;
using Microsoft.AspNetCore.Mvc;
using API.Casino.Services.Interfaces;
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
}