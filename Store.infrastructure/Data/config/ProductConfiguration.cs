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
  public class ProductConfiguration : IEntityTypeConfiguration<Product>
  {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(50);

      builder.Property(x => x.Description)
        .IsRequired()
        .HasMaxLength(500);
      
      builder.Property(x => x.Price)
        .IsRequired()
        .HasColumnType("decimal(18,2)");
      
      builder.HasMany(x => x.Photos)
        .WithOne(x => x.Product)
        .HasForeignKey(x => x.ProductId)
        .OnDelete(DeleteBehavior.Cascade);
      

      builder.HasData(
        new Product
        {
          Id = 1,
          Name = "Laptop",
          Description = "Gaming laptop",
          Price = 25000,
          CategoryId = 1 
        },
    new Product
    {
      Id = 2,
      Name = "Novel",
      Description = "Fiction book",
      Price = 150,
      CategoryId = 2
    }
    );

    }
  }
}
