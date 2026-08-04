using Credit_Wallet.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Credit_Wallet.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Wallet>()
            .Property(w => w.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Wallet>()
            .Property(w=>w.LastUpdateDateTime)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
        

        base.OnModelCreating(modelBuilder);
    }
}
