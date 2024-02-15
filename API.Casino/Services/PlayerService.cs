using API.Casino.Models;
using API.Casino.Services.Interfaces;
using Common.Utils.Data.Interfaces;

namespace API.Casino.Services;

public class PlayerService : IPlayerService
{
    private readonly IRepository<Player> _playerRepository;

    public PlayerService(IRepository<Player> playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Player> SetupPlayerAsync(string telegramUsername, string referralCode)
    {
        var existingPlayer =
            await _playerRepository.GetByPredicateAsync(player => player.TelegramUsername == telegramUsername);
        if (existingPlayer != null)
        {
            return existingPlayer;
        }

        var newPlayer = new Player
            { TelegramUsername = telegramUsername, ReferralCode = new Guid().ToString(), Spins = 10, Score = 0 };
        await _playerRepository.AddAsync(newPlayer);

        return newPlayer;
    }

    public async Task<Player> IncreasePlayerScoreAsync(string username, int points)
    {
        // @TODO: Implement spin logic in separate service
        await Task.Yield();
        return new Player();
    }
}