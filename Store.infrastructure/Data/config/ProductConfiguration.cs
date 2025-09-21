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
      new Product { Id = 1, Name = "Bio Soft Deep Conditioner", Description = "Deep nourishing conditioner 500g", Stock = 50, OldPrice = 180, NewPrice = 150, CategoryId = 2, AverageRating = 4.5, ReviewCount = 20 },
      new Product { Id = 2, Name = "Bobai Extra Lightening Sun Screen", Description = "Extra lightening sunscreen gel 50gm", Stock = 40, OldPrice = 160, NewPrice = 140, CategoryId = 1, AverageRating = 4.2, ReviewCount = 15 },
      new Product { Id = 3, Name = "Dermatique Sun Mattifying Fluid", Description = "Mattifying sun fluid 50ml", Stock = 60, OldPrice = 180, NewPrice = 160, CategoryId = 1, AverageRating = 4.7, ReviewCount = 30 },
      new Product { Id = 4, Name = "Essence Juicy Melon Lip Balm", Description = "Tinted lip cheek balm", Stock = 70, OldPrice = 100, NewPrice = 80, CategoryId = 3, AverageRating = 4.0, ReviewCount = 12 },
      new Product { Id = 5, Name = "Lebelage Truly Serum", Description = "Truly skin serum 35ml", Stock = 30, OldPrice = 140, NewPrice = 120, CategoryId = 1, AverageRating = 4.3, ReviewCount = 18 },
      new Product { Id = 6, Name = "Leylak Eye Contour Gel", Description = "Eye contour gel 15ml", Stock = 25, OldPrice = 130, NewPrice = 110, CategoryId = 1, AverageRating = 4.1, ReviewCount = 10 },
      new Product { Id = 7, Name = "Loreal Brown Magic Retouch", Description = "Magic retouch 75ml", Stock = 80, OldPrice = 120, NewPrice = 100, CategoryId = 3, AverageRating = 4.6, ReviewCount = 25 },
      new Product { Id = 8, Name = "Moist 1 Cream", Description = "Moisturizing cream 100g", Stock = 100, OldPrice = 150, NewPrice = 130, CategoryId = 1, AverageRating = 4.4, ReviewCount = 40 },
      new Product { Id = 9, Name = "Olaplex No.3 Hair Perfector", Description = "Hair perfector 100ml", Stock = 45, OldPrice = 230, NewPrice = 200, CategoryId = 2, AverageRating = 4.8, ReviewCount = 35 },
      new Product { Id = 10, Name = "ORS Argan Oil Spray", Description = "Argan oil spray", Stock = 55, OldPrice = 170, NewPrice = 150, CategoryId = 2, AverageRating = 4.5, ReviewCount = 22 },
      new Product { Id = 11, Name = "Purederm Black Bubble Mask", Description = "Black bubble mask", Stock = 90, OldPrice = 90, NewPrice = 70, CategoryId = 1, AverageRating = 4.0, ReviewCount = 12 },
      new Product { Id = 12, Name = "Raw African Follicle Booster Oil", Description = "Follicle booster oil 100ml", Stock = 35, OldPrice = 190, NewPrice = 160, CategoryId = 2, AverageRating = 4.6, ReviewCount = 28 },
      new Product { Id = 13, Name = "Seropipe Hair Shampoo", Description = "Shampoo 300ml", Stock = 60, OldPrice = 160, NewPrice = 140, CategoryId = 2, AverageRating = 4.3, ReviewCount = 19 },
      new Product { Id = 14, Name = "Shaan Make Up Remover", Description = "Make-up remover 200ml", Stock = 50, OldPrice = 110, NewPrice = 90, CategoryId = 1, AverageRating = 4.2, ReviewCount = 15 },
      new Product { Id = 15, Name = "Sheglam Complexion Boost Concealer", Description = "Boost concealer Acorn", Stock = 65, OldPrice = 130, NewPrice = 110, CategoryId = 3, AverageRating = 4.4, ReviewCount = 23 },
      new Product { Id = 16, Name = "Sheglam Jelly Licious Lip Blush", Description = "Hydrating lip blush tint", Stock = 75, OldPrice = 120, NewPrice = 100, CategoryId = 3, AverageRating = 4.3, ReviewCount = 20 },
      new Product { Id = 17, Name = "Sheglam Liquid Blush Petal Talk", Description = "Liquid blush Petal Talk", Stock = 55, OldPrice = 115, NewPrice = 95, CategoryId = 3, AverageRating = 4.2, ReviewCount = 18 },
      new Product { Id = 18, Name = "Sheglam Setting Powder Duo", Description = "Setting powder duo Bisque", Stock = 50, OldPrice = 125, NewPrice = 105, CategoryId = 3, AverageRating = 4.1, ReviewCount = 15 },
      new Product { Id = 19, Name = "Wet n Wild Foundation", Description = "Photo focus foundation soft ivory", Stock = 70, OldPrice = 200, NewPrice = 170, CategoryId = 3, AverageRating = 4.5, ReviewCount = 26 },
      new Product { Id = 20, Name = "Maybelline Fit Me Concealer", Description = "Concealer Sand 20", Stock = 65, OldPrice = 170, NewPrice = 150, CategoryId = 3, AverageRating = 4.6, ReviewCount = 30 },
      new Product { Id = 21, Name = "Shaan Body Milk", Description = "Body milk 300ml", Stock = 60, OldPrice = 140, NewPrice = 120, CategoryId = 4, AverageRating = 4.2, ReviewCount = 18 },
      new Product { Id = 22, Name = "Mood Shower Gel", Description = "Shower gel 750ml", Stock = 75, OldPrice = 160, NewPrice = 140, CategoryId = 4, AverageRating = 4.3, ReviewCount = 22 },
      new Product { Id = 23, Name = "Skin Candy Perfumed Hair Body Oil", Description = "Perfumed hair & body oil 50ml", Stock = 40, OldPrice = 190, NewPrice = 160, CategoryId = 4, AverageRating = 4.6, ReviewCount = 25 },
      new Product { Id = 24, Name = "Starville Roll On", Description = "Roll-on deodorant", Stock = 100, OldPrice = 110, NewPrice = 90, CategoryId = 4, AverageRating = 4.0, ReviewCount = 12 },
      new Product { Id = 25, Name = "Bodylicious Body Lotion", Description = "Body lotion 236ml", Stock = 55, OldPrice = 170, NewPrice = 150, CategoryId = 4, AverageRating = 4.4, ReviewCount = 20 },
      new Product { Id = 26, Name = "Watsons Body Mousse", Description = "Body mousse 75ml", Stock = 45, OldPrice = 150, NewPrice = 130, CategoryId = 4, AverageRating = 4.1, ReviewCount = 14 },
      new Product { Id = 27, Name = "Skin Candy Deodorant Cream", Description = "Deodorant cream", Stock = 50, OldPrice = 130, NewPrice = 110, CategoryId = 4, AverageRating = 4.2, ReviewCount = 16 }
  );



    }
  }
}

