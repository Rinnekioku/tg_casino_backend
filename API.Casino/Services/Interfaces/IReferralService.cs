using API.Casino.DTOs.Referral;

namespace API.Casino.Services.Interfaces;

public interface IReferralService
{
    // @TODO: Implement after figuring out how to get user id from JWT Token
    // public Task<List<Referral>> GetReferrals();

    public Task ReferPlayer(string telegramUsername, string referralCode);

    public Task TryApproveReferral(string telegramUsername);

    public Task<string> GenerateReferralLink(string telegramUsername);
}