using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.ProductEntity;
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
