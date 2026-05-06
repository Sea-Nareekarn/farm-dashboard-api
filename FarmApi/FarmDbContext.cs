using Microsoft.EntityFrameworkCore;
using FarmApi.Models;

namespace FarmApi.Data;

public class FarmDbContext(DbContextOptions<FarmDbContext> options) : DbContext(options)
{
    public DbSet<FarmDashboardTransactionDb> FarmDashboardTransactionDb => Set<FarmDashboardTransactionDb>();
    public DbSet<MasTypeDb> MasTypeDb => Set<MasTypeDb>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MasTypeDb>()
            .HasKey(m => new { m.GroupCode, m.Code });
        
        base.OnModelCreating(modelBuilder);
    }
}
