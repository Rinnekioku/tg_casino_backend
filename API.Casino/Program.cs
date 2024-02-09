using API.Casino.EventHandlers;
using Common.CasinoServices.Data;
using Common.CasinoServices.Services;
using Common.CasinoServices.Services.Interfaces;
using Common.Utils.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<TelegramLoginHandler>();
builder.Services.AddUtilityServices(builder.Configuration);
builder.Services.AddDbContext<CasinoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("API.Casino")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();