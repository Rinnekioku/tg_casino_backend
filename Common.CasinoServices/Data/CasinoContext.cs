using Common.CasinoServices.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.CasinoServices.Data;

public class CasinoContext : DbContext
{
    public CasinoContext(DbContextOptions<CasinoContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
}