using API.Casino.Models;

namespace API.Casino.Services.Interfaces;

public interface IPlayerService
{
    public Task<Player> SetupPlayerAsync(string telegramUsername);
}