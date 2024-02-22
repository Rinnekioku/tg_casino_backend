using System.IdentityModel.Tokens.Jwt;
using System.Text;
using API.Casino.Models;
using API.Casino.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Common.Utils.Data.Interfaces;

namespace API.Casino.Services;

public class AccountService : IAccountService
{
    private readonly IPlayerService _playerService;
    private IConfiguration _configuration;

    public AccountService(IRepository<Player> playerRepository, IPlayerService playerService,
        IConfiguration configuration)
    {
        _playerService = playerService;
        _configuration = configuration;
    }

    // @TODO: Make proper referral system
    public async Task<string> TelegramLogin(string telegramUsername)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var secToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(secToken);

        await OnFirstLogin(telegramUsername);

        return token;
    }

    private async Task OnFirstLogin(string telegramUsername)
    {
        await _playerService.SetupPlayerAsync(telegramUsername);
    }
}