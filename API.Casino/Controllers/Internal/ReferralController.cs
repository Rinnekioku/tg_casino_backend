using Common.Utils.DTOs.Account;
using API.Casino.Services.Interfaces;
using Common.Utils.DTOs.Referral;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Casino.Controllers.Internal;

[Route("api/internal/[controller]")]
[ApiController]
public class ReferralController : ControllerBase
{
    private readonly IReferralService _referralService;

    public ReferralController(IReferralService referralService)
    {
        _referralService = referralService;
    }

    [HttpPost("TelegramReferPlayer")]
    public async Task<ActionResult> TelegramReferPlayer([FromBody] TelegramReferPlayerRequest request)
    {
        await _referralService.ReferPlayer(request.TelegramUsername, request.ReferralCode);

        return Ok();
    }

    [HttpPost("TelegramGenerateReferralLink")]
    public async Task<ActionResult<TelegramGenerateReferralLinkResponse>> TelegramGenerateReferralLink(
        [FromBody] TelegramGenerateReferralLinkRequest request)
    {
        return Ok(new TelegramGenerateReferralLinkResponse
            { ReferralLink = await _referralService.GenerateReferralLink(request.TelegramUsername) });
    }
}