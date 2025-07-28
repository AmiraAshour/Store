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
  public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
  {
    public void Configure(EntityTypeBuilder<Photo> builder)
    {

      builder.HasData(
        new Photo { Id = 1, ImageName = "laptop1.jpg", ProductId = 1 },
        new Photo { Id = 2, ImageName = "phone1.jpg", ProductId = 2 }
      );
    }
  }
}
