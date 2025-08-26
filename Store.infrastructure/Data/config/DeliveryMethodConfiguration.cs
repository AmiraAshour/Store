using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.infrastructure.Data.config
{
  public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
  {
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
      builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
      builder.HasData(
        new DeliveryMethod
        {
          Id = 1,
          Name = "Fast",
          Price = 10,
          DeliveryTime = "1-2 days",
          Description = "Fast delivery"
        },
    new DeliveryMethod
    {
      Id = 2,
      Name = "Slow",
      Price = 5,
      DeliveryTime = "5-7 days",
      Description = "Economy delivery"
    }
        );
    }
  }
}
