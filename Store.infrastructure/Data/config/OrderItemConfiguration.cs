using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order;

namespace Store.infrastructure.Data.config
{
  public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
  {
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
      builder.Property(x => x.Price).HasColumnType("decimal(18,2)");


      //builder.HasData(
      //    new OrderItem { OrderId=1, Id = 1, ProductItemId = 1, ProductName = "Bio Soft Deep Conditioner", MainImage = "images/Bio Soft Deep Conditioner/bio-soft-deep-conditioner-500g.jpg", Price = 150, Quntity = 2 },
         
      //    new OrderItem {OrderId=1, Id = 2, ProductItemId = 2, ProductName = "Bio Soft Shampoo", MainImage = "images/Bio Soft Shampoo/bio-soft-shampoo-500ml.jpg", Price = 120, Quntity = 5 },
         
      //    new OrderItem {OrderId=1, Id = 3, ProductItemId = 3, ProductName = "Dermatique Sun Mattifying Fluid", MainImage = "images/Dermatique Sun/dermatique-sun-mattifying-fluid-50ml.jpg", Price = 160, Quntity = 1 },
                    
      //    new OrderItem {OrderId=1, Id = 4, ProductItemId = 4, ProductName = "Dermatique Hydrating Cream", MainImage = "images/Dermatique Hydrating/dermatique-hydrating-cream-100ml.jpg", Price = 180, Quntity = 4 },
          
      //    new OrderItem {OrderId=1, Id = 5, ProductItemId = 5, ProductName = "L’Oreal Serum", MainImage = "images/LOreal/loreal-serum-50ml.jpg", Price = 200, Quntity = 3 },
          
      //    new OrderItem { OrderId=2, Id = 6, ProductItemId = 6, ProductName = "L’Oreal Conditioner", MainImage = "images/LOreal/loreal-conditioner-250ml.jpg", Price = 140, Quntity = 6 },
          
      //    new OrderItem {OrderId=2, Id = 7, ProductItemId = 7, ProductName = "The Ordinary Niacinamide", MainImage = "images/The Ordinary/niacinamide-30ml.jpg", Price = 220, Quntity = 7 },
          
      //    new OrderItem {OrderId=2, Id = 8, ProductItemId = 8, ProductName = "The Ordinary Hyaluronic Acid", MainImage = "images/The Ordinary/hyaluronic-30ml.jpg", Price = 210, Quntity = 2 },
          
      //    new OrderItem {OrderId=2, Id = 9, ProductItemId = 9, ProductName = "Olaplex No.3 Hair Perfector", MainImage = "images/Olaplex/olaplex-no.3-hair-perfector-100ml.png", Price = 200, Quntity = 3 },
          
      //    new OrderItem {OrderId=2, Id = 10, ProductItemId = 10, ProductName = "Olaplex No.6 Bond Smoother", MainImage = "images/Olaplex/olaplex-no.6-bond-smoother-100ml.png", Price = 230, Quntity = 8 }
      //);
    }
  }
}
