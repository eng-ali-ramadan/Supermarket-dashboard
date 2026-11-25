using Microsoft.EntityFrameworkCore;
using SupermarketDashboard.Models;

namespace SupermarketDashboard.Data;

public class SupermarketContext : DbContext
{
    public SupermarketContext(DbContextOptions<SupermarketContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // دقة الفلوس: 10 أرقام إجمالاً، منها 2 عشرية (مثلاً 99999999.99)
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Sale>().Property(s => s.UnitPrice).HasPrecision(10, 2);
        modelBuilder.Entity<Expense>().Property(e => e.Amount).HasPrecision(10, 2);
    }
}