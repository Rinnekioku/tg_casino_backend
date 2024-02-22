using API.Casino.Enums;
using API.Casino.Models;
using API.Casino.Services.Interfaces;
using Common.Utils.Data.Interfaces;

namespace API.Casino.Services;

public class ReferralService : IReferralService
{
    private readonly ILogger<ReferralService> _logger;
    private readonly IRepository<Player> _playerRepository;

    public ReferralService(IRepository<Player> playerRepository, ILogger<ReferralService> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public async Task ReferPlayer(string telegramUsername, string referralCode)
    {
        if (await IsReferringInvalidPlayer(telegramUsername, referralCode)) return;

        if (await _playerRepository.ContainsByPredicateAsync(player => player.TelegramUsername == telegramUsername))
        {
            _logger.LogInformation(
                "Player with [{0}] TelegramUsername already registered, only new players can use referral code",
                telegramUsername);
            return;
        }

        var referrer =
            await _playerRepository.GetByPredicateAsync(player => player.ReferralCode == referralCode);
        var referral = new Player
        {
            TelegramUsername = telegramUsername,
            ReferralCode = Guid.NewGuid().ToString(),
            Spins = 10,
            Score = 0,
            IsSetupComplete = false,
            ReferrerId = referrer!.Id,
            ReferralStatus = ReferralStatus.Pending.ToString()
        };

        await _playerRepository.AddAsync(referral);
    }

    public async Task TryApproveReferral(string telegramUsername)
    {
        var referral = await _playerRepository.GetByPredicateAsync(referral =>
            referral.TelegramUsername == telegramUsername);
        if (referral?.ReferralStatus == ReferralStatus.Unreferred.ToString())
        {
            _logger.LogInformation("Player with {TelegramUsername} telegram username don't have referral code",
                telegramUsername);
            return;
        }

        referral.ReferralStatus = ReferralStatus.Approved.ToString();

        await _playerRepository.UpdateAsync(referral);
    }

    public async Task<string> GenerateReferralLink(string telegramUsername)
    {
        var referralCode =
            (await _playerRepository.GetByPredicateAsync(player => player.TelegramUsername == telegramUsername))!
            .ReferralCode;

        return $"https://t.me/affiliate_casino_bot?start={referralCode}";
    }

    private async Task<bool> IsReferringInvalidPlayer(string telegramUsername, string referralCode)
    {
        if (referralCode == string.Empty)
        {
            return false;
        }

        var referrer = await _playerRepository.GetByPredicateAsync(player => player.ReferralCode == referralCode);
        var referral =
            await _playerRepository.GetByPredicateAsync(player => player.TelegramUsername == telegramUsername);

        if (referral is null || referrer is null)
        {
            _logger.LogError("Referral or referrer not found");
            return false;
        }

        if (referral.Id == referrer.Id)
        {
            _logger.LogError("Player cannot refer himself, player id {PlayerId}", referral.Id);
            return false;
        }

        return true;
    }
}