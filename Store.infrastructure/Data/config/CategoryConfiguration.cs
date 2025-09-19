using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.ProductEntity;

namespace Store.infrastructure.Data.config
{
  public class CategoryConfiguration : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      builder.Property(x=>x.Name).IsRequired().HasMaxLength(30);
      builder.Property(x => x.Description).IsRequired();
      builder.HasData(
          new Category { Id = 1, Name = "Skin Care", Description = "Skin care products" },
          new Category { Id = 2, Name = "Hair Care", Description = "Hair care products" },
          new Category { Id = 3, Name = "Makeup", Description = "Makeup and cosmetics" },
          new Category { Id = 4, Name = "Body Care", Description = "Body care products" }
    );
    }
  }
}
