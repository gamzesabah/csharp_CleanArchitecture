using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Products;

internal sealed class ProductConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2);

        builder.Property(x => x.Stock)
            .IsRequired();

        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.Category);

        builder.HasIndex(x => x.Price);

        builder.HasIndex(x => new
        {
            x.Category,
            x.Price
        });
    }
}
