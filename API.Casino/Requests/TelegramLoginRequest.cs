namespace API.Casino.Requests;

public class TelegramLoginRequest
{
    public required string TelegramUsername { get; set; }

    public string ReferralCode { get; set; } = "";
}