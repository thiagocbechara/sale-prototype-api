using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleApiPrototype.Infra.Database.Entities;

namespace SaleApiPrototype.Infra.Database.EntitiesConfigs;

internal class SaleConfiguration : IEntityTypeConfiguration<SaleDb>
{
    public void Configure(EntityTypeBuilder<SaleDb> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.SaleNumber);

        builder.Property(s => s.TotalAmount).HasPrecision(38, 2);
        builder.Property(s => s.Customer).IsRequired();
        builder.Property(s => s.Branch).IsRequired();
        builder.Property(s => s.IsCancelled).HasDefaultValue(false);

        builder.HasMany(s => s.Products)
            .WithOne(p => p.Sale)
            .HasForeignKey(p => p.SaleNumber)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}
