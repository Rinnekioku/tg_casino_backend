using API.Casino.Services.Interfaces;
using Common.Utils.DTOs.Leaderboard;
using Microsoft.AspNetCore.Mvc;

namespace API.Casino.Controllers.External;

[Route("api/external/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpPost("GetPlayerRank")]
    public async Task<ActionResult<GetPlayerRankResponse>> GetPlayerRank(
        [FromBody] GetPlayerRankRequest request)
    {
        var playerRank = await _leaderboardService.GetPlayerRank(request.TelegramUsername);

        if (playerRank is null)
        {
            return BadRequest();
        }

        return Ok(new GetPlayerRankResponse
            { Rank = playerRank.Item1, TelegramUsername = playerRank.Item2.TelegramUsername });
    }

    [HttpPost("GetLeaderboardPage")]
    public async Task<ActionResult<GetLeaderboardPageResponse>> GetLeaderboardPage(
        /*[FromBody] GetLeaderboardPageRequest request*/)
    {
        var response = new GetLeaderboardPageResponse
        {
            Players = (await _leaderboardService.GetLeaderboardPage(0)).Select((p, i) =>
                new LeaderboardRow { Rank = i, TelegramUsername = p.TelegramUsername, Score = p.Score }).ToList()
        };

        return Ok(response);
    }
}