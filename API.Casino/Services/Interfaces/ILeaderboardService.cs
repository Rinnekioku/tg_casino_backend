using API.Casino.Models;

namespace API.Casino.Services.Interfaces;

public interface ILeaderboardService
{
    public Task<Tuple<int, Player>?> GetPlayerRank(string telegramUsername);
    public Task<List<Player>> GetLeaderboardPage(int offset, int limit = 20);
}