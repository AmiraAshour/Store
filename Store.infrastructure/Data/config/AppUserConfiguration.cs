

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities;

namespace Store.infrastructure.Data.config
{
  public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
  {
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {

      builder.HasMany(x => x.Address).WithOne(a => a.AppUser).HasConstraintName("FK_AppUser_Address").OnDelete(DeleteBehavior.Cascade);
    }
  }
}
