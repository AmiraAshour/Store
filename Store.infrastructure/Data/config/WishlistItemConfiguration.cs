

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities.comman;
using System.Reflection.Emit;

namespace Store.infrastructure.Data.config
{
  public class WishlistItemConfiguration: IEntityTypeConfiguration<WishlistItem>
  {
    public void Configure(EntityTypeBuilder<WishlistItem> builder)
    {
      builder.HasKey(x => new { x.ProductId, x.UserId });
   

      builder.HasOne(w => w.User)
          .WithMany()
          .HasForeignKey(w => w.UserId);

      builder.HasOne(w => w.Product)
          .WithMany()
          .HasForeignKey(w => w.ProductId);
    }
 
  }
}
