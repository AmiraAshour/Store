using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using Store.Core.Entities.Order;
using System.Reflection.Emit;

namespace Store.infrastructure.Data.config
{
  public class OrderConfiguration : IEntityTypeConfiguration<Orders>
  {

    public void Configure(EntityTypeBuilder<Orders> builder)
    {
      builder.OwnsOne(builder => builder.shippingAddress, a => a.WithOwner());

      builder.HasMany(o => o.orderItems)
       .WithOne(i => i.Order)
       .HasForeignKey(i => i.OrderId) 
       .OnDelete(DeleteBehavior.Cascade);


      builder.Property(x => x.status).HasConversion(x => x.ToString(), o => (Status)Enum.Parse(typeof(Status), o));
      builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");


      
    }
  }
}
