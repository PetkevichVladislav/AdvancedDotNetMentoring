using CatalogService.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.DataAccess.DatabaseContexts.MsSql.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(product => product.Id);
            builder.Property(product => product.Name).IsRequired().HasColumnType("NVARCHAR(50)");
            builder.Property(product => product.Description).IsRequired(false).HasColumnType("NVARCHAR(MAX)");
            builder.Property(product => product.Image).IsRequired(false).HasColumnType("NVARCHAR(MAX)");
            builder.Property(product => product.Price).IsRequired().HasColumnType("MONEY");
            builder.Property(product => product.Price).IsRequired().HasColumnType("INT");

            builder.HasOne(product => product.Category)
                .WithOne()
                .HasForeignKey<Product>(product => product.CategoryId);
        }
    }
}
