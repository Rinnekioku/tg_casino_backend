using Common.Utils.DTOs.Account;
using API.Casino.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Casino.Controllers.Internal;

[Route("api/internal/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("TelegramLogin")]
    public async Task<ActionResult<TelegramLoginResponse>> TelegramLogin([FromBody] TelegramLoginRequest request)
    {
        return Ok(new TelegramLoginResponse
            { Token = await _accountService.TelegramLogin(request.TelegramUsername) });
    }
}