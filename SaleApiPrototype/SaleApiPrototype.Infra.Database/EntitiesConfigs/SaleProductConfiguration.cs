using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaleApiPrototype.Infra.Database.Entities;

namespace SaleApiPrototype.Infra.Database.EntitiesConfigs;

internal class SaleProductConfiguration : IEntityTypeConfiguration<SaleProductDb>
{
    public void Configure(EntityTypeBuilder<SaleProductDb> builder)
    {
        builder.ToTable("SaleProducts");
        builder.HasKey(p => p.Id);

        builder
            .HasIndex(p => new { p.SaleNumber, p.ProductName })
            .IsUnique();

        builder.Property(s => s.UnitPrice).HasPrecision(38, 2);
        builder.Property(s => s.Discount).HasPrecision(7, 5);
        builder.Property(s => s.TotalAmount).HasPrecision(38, 2);
        builder.Property(p => p.ProductName).IsRequired();

        builder.HasOne(p => p.Sale)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SaleNumber)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
