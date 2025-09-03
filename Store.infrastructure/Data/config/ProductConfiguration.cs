using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.ProductEntity;

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
      
      builder.Property(x => x.NewPrice)
        .IsRequired()
        .HasColumnType("decimal(18,2)");
      
      
      builder.Property(x => x.OldPrice)
        .HasColumnType("decimal(18,2)");
      
    
      

      builder.HasData(
        new Product
        {
          Id = 1,
          Name = "Laptop",
          Description = "Gaming laptop",
          NewPrice = 25000,
          CategoryId = 1 
        },
    new Product
    {
      Id = 2,
      Name = "Novel",
      Description = "Fiction book",
      NewPrice = 150,
      CategoryId = 2
    }
    );

    }
  }
}
  
