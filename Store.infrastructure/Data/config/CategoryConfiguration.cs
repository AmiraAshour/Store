using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.infrastructure.Data.config
{
  public class CategoryConfiguration : IEntityTypeConfiguration<Category>
  {
    public void Configure(EntityTypeBuilder<Category> builder)
    {
      builder.Property(x=>x.Name).IsRequired().HasMaxLength(30);
      builder.Property(x => x.Description).IsRequired();
      builder.HasData(
        new Category { Id = 1, Name = "Laptops", Description = "Electronic laptops" },
        new Category { Id = 2, Name = "Phones", Description = "Smart mobile phones" }
    );
    }
  }
}
