using API.Casino.Exceptions;
using API.Casino.Models;
using API.Casino.Repositories.Iterfaces;
using API.Casino.Services.Interfaces;
using Common.Utils.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Casino.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly ILeaderboardRepository _leaderboardRepository;
    private readonly IRepository<Player> _playerRepository;

    public LeaderboardService(ILeaderboardRepository leaderboardRepository, IRepository<Player> playerRepository)
    {
        _leaderboardRepository = leaderboardRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Tuple<int, Player>?> GetPlayerRank(string telegramUsername)
    {
        return await _leaderboardRepository.GetPlayerRankAsync(telegramUsername);
    }

    public async Task<List<Player>> GetLeaderboardPage(int offset, int limit = 20)
    {
        return await _leaderboardRepository.GetLeaderboardPageAsync(offset, limit);
    }
}