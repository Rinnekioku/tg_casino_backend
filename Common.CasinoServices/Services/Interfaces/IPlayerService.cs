using Common.CasinoServices.Models;

namespace Common.CasinoServices.Services.Interfaces;

public interface IPlayerService
{
    public Task<Player> RegisterPlayer(string username);
    public Task<Player> IncreasePlayerScore(string username, int points);
}