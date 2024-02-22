using API.Casino.Models;
using API.Casino.Repositories.Iterfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Casino.Repositories;

public class LeaderboardRepository : ILeaderboardRepository
{
    private readonly DbContext _context;

    public LeaderboardRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<Tuple<int, Player>?> GetPlayerRankAsync(string telegramUsername)
    {
        var player = await _context.Set<Player>()
            .FirstOrDefaultAsync(p => p.TelegramUsername == telegramUsername);

        if (player is null)
        {
            return null;
        }

        var rank = await _context.Set<Player>()
            .Where(p => p.Score <= player.Score)
            .CountAsync();

        // Return a tuple containing the rank and player information
        return new Tuple<int, Player>(rank, player);
    }

    public async Task<List<Player>> GetLeaderboardPageAsync(int limit, int offset)
    {
        return await _context
            .Set<Player>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}