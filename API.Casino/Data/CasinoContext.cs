using API.Casino.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Casino.Data;

public class CasinoContext : DbContext
{
    public CasinoContext(DbContextOptions<CasinoContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
}