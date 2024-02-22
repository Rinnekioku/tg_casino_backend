using API.Casino.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Casino.Data;

public class CasinoContext : DbContext
{
    public CasinoContext(DbContextOptions<CasinoContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Player>()
            .HasOne(e => e.Referrer)
            .WithMany(e => e.Referrals)
            .HasForeignKey(e => e.ReferrerId)
            .IsRequired(false);
    }

    public DbSet<Player> Players { get; set; }
}