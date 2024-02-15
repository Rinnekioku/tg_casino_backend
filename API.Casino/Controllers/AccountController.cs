using API.Casino.Requests;
using API.Casino.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Casino.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("TelegramLogin")]
    public async Task<IActionResult> TelegramLogin([FromBody] TelegramLoginRequest request)
    {
        var result = new
            { token = await _accountService.TelegramLogin(request.TelegramUsername, request.ReferralCode) };
        return Ok(result);
    }
}