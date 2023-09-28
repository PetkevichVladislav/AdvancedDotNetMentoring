using CatalogService.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.DataAccess.DatabaseContexts.MsSql.Configuration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(category => category.Id);
            builder.Property(category => category.Name).IsRequired().HasColumnType("NVARCHAR(50)");
            builder.Property(category => category.Image).IsRequired(false).HasColumnType("NVARCHAR(MAX)");
            builder.Property(category => category.ParentCategoryId).IsRequired(false).HasColumnType("INT");

            builder.HasOne(category => category.ParentCategory)
                .WithOne()
                .HasForeignKey<Category>(category => category.ParentCategoryId);
        }
    }
}
