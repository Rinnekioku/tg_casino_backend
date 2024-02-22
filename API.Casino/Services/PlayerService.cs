using API.Casino.Enums;
using API.Casino.Models;
using API.Casino.Services.Interfaces;
using Common.Utils.Data.Interfaces;

namespace API.Casino.Services;

public class PlayerService : IPlayerService
{
    private readonly IRepository<Player> _playerRepository;
    private readonly IReferralService _referralService;

    public PlayerService(IRepository<Player> playerRepository, IReferralService referralService)
    {
        _playerRepository = playerRepository;
        _referralService = referralService;
    }

    public async Task<Player> SetupPlayerAsync(string telegramUsername)
    {
        Player? player =
            await _playerRepository.GetByPredicateAsync(player => player.TelegramUsername == telegramUsername);
        if (player?.IsSetupComplete ?? false)
        {
            return player;
        }

        if (player is null)
        {
            player = new Player
            {
                TelegramUsername = telegramUsername,
                ReferralCode = Guid.NewGuid().ToString(),
                Spins = 10,
                Score = 0,
                IsSetupComplete = true,
                ReferralStatus = ReferralStatus.Unreferred.ToString()
            };
            await _playerRepository.AddAsync(player);
        }
        else
        {
            player.IsSetupComplete = true;
            await _playerRepository.UpdateAsync(player);
        }

        await _referralService.TryApproveReferral(telegramUsername);

        return player;
    }
}