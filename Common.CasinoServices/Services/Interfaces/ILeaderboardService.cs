using Common.CasinoServices.Models;

namespace Common.CasinoServices.Services.Interfaces;

public interface ILeaderboardService
{
    public Task<Tuple<Player, int>> GetPlayerRank(string username);
    public Task<List<Player>> GetLeaderboard();
}