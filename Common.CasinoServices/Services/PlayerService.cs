using Common.CasinoServices.Data;
using Common.CasinoServices.Exceptions;
using Common.CasinoServices.Models;
using Common.CasinoServices.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.CasinoServices.Services;

public class PlayerService : IPlayerService
{
    private readonly CasinoContext _casinoContext;

    public PlayerService(CasinoContext casinoContext)
    {
        _casinoContext = casinoContext;
    }

    public async Task<Player> RegisterPlayer(string username)
    {
        if (await _casinoContext.Players.FirstOrDefaultAsync(player => player.Username == username) != null)
        {
            throw new AlreadyExistsException($"Player with ${username} already exists");
        }

        var player = new Player { Username = username };
        await _casinoContext.Players.AddAsync(player);
        await _casinoContext.SaveChangesAsync();
        return player;
    }

    public async Task<Player> IncreasePlayerScore(string username, int points)
    {
        var player = await _casinoContext.Players.FirstOrDefaultAsync(player => player.Username == username);
        if (player == null)
        {
            throw new NotFoundException($"Player with ${username} not found");
        }

        player.Score += points;
        _casinoContext.Players.Update(player);
        await _casinoContext.SaveChangesAsync();
        return player;
    }
}