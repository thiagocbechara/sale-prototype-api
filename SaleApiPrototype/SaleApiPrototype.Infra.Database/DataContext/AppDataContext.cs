using Microsoft.EntityFrameworkCore;
using SaleApiPrototype.Infra.Database.Entities;

namespace SaleApiPrototype.Infra.Database.DataContext;

internal class AppDataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<SaleDb> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataContext).Assembly);
    }
}
