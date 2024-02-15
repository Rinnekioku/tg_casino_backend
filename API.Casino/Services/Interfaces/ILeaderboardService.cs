using API.Casino.Models;

namespace API.Casino.Services.Interfaces;

public interface ILeaderboardService
{
    public Task<Tuple<Player, int>> GetPlayerRank(string username);
    public Task<List<Player>> GetLeaderboard();
}