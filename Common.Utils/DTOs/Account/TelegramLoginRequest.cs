namespace Common.Utils.DTOs.Account;

public class TelegramLoginRequest
{
    public required string TelegramUsername { get; set; }

    public string ReferralCode { get; set; } = "";
}