using API.Casino.Models;

namespace API.Casino.Repositories.Iterfaces;

public interface ILeaderboardRepository
{
    public Task<Tuple<int, Player>?> GetPlayerRankAsync(string telegramUsername);
    
    public Task<List<Player>> GetLeaderboardPageAsync(int limit, int offset);
}