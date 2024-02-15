using System.Text;
using API.Casino.Data;
using API.Casino.Messaging.Handlers;
using API.Casino.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Casino.Services;
using API.Casino.Services.Interfaces;
using Common.Utils.Data;
using Common.Utils.Data.Interfaces;
using Common.Utils.Extensions;
using Common.Utils.Messaging.Events;
using Common.Utils.Messaging.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TelegramLoginRequestHandler>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddUtilityServices(builder.Configuration);

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });
builder.Services.AddDbContext<DbContext, CasinoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("API.Casino")));

builder.Services.AddScoped<IRepository<Player>, EntityFrameworkRepository<Player>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Services.GetRequiredService<IEventBus>()
    .On<TelegramLoginRequest, TelegramLoginRequestHandler>();

app.Run();