namespace API.Casino.Services.Interfaces;

public interface IAccountService
{
    public Task<string> TelegramLogin(string telegramUsername);
}