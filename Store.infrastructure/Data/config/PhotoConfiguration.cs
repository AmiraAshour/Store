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
          new Photo { Id = 1, ImageName = "images/Bio Soft Deep Conditioner/bio-soft-deep-conditioner-500g.jpg", ProductId = 1 },
          new Photo { Id = 2, ImageName = "images/Bobai Sun Screen/bobai-extra-lightening-sun-screen-gel-50gm.jpg", ProductId = 2 },
          new Photo { Id = 3, ImageName = "images/Dermatique Sun/dermatique-sun-mattifying-fluid-50ml.jpg", ProductId = 3 },
          new Photo { Id = 4, ImageName = "images/Essence Lip Balm/essence-juicy-melon-tinted-lip-cheek-balm.jpg", ProductId = 4 },
          new Photo { Id = 5, ImageName = "images/Lebelage Serum/lebelage-truly-serum-35ml.jpg", ProductId = 5 },
          new Photo { Id = 6, ImageName = "images/Leylak Eye Gel/leylak-eye-contour-gel-15ml.jpg", ProductId = 6 },
          new Photo { Id = 7, ImageName = "images/Loreal Retouch/loreal-brown-magic-retouch-75ml.png", ProductId = 7 },
          new Photo { Id = 8, ImageName = "images/Moist 1 Cream/moist-1-cream-moisturizing-cream-100g.jpg", ProductId = 8 },
          new Photo { Id = 9, ImageName = "images/Olaplex/olaplex-no.3-hair-perfector-100ml.png", ProductId = 9 },
          new Photo { Id = 10, ImageName = "images/ORS Argan Oil/ors-argan-oil-spray.png", ProductId = 10 },
          new Photo { Id = 11, ImageName = "images/Purederm Mask/purederm-black-bubble-mask.png", ProductId = 11 },
          new Photo { Id = 12, ImageName = "images/Raw African Oil/raw-african-follicle-booster-oil-100ml.jpg", ProductId = 12 },
          new Photo { Id = 13, ImageName = "images/Seropipe Shampoo/seropipe-hair-shampoo-300ml.jpg", ProductId = 13 },
          new Photo { Id = 14, ImageName = "images/Shaan Remover/shaan-make-up-remover-200ml-600x600.jpg", ProductId = 14 },
          new Photo { Id = 15, ImageName = "images/Sheglam Concealer/sheglam-complexation-boost-concealer-acorn.jpg", ProductId = 15 },
          new Photo { Id = 16, ImageName = "images/Sheglam Lip Blush/sheglam-jelly-licious-hydrating-lip-blush-tint-aho.jpg", ProductId = 16 },
          new Photo { Id = 17, ImageName = "images/Sheglam Liquid Blush/sheglam-liquid-blush-petal-talk.jpg", ProductId = 17 },
          new Photo { Id = 18, ImageName = "images/Sheglam Powder/sheglam-setting-powder-duo-bisque-600x601.jpg", ProductId = 18 },
          new Photo { Id = 19, ImageName = "images/Wet n Wild/wet-n-wild-photofocus-foundation-362-soft-ivory.jpg", ProductId = 19 },
          new Photo { Id = 20, ImageName = "images/Maybelline Concealer/maybelline-fit-me-concealer-20-sand.jpg", ProductId = 20 },
          new Photo { Id = 21, ImageName = "images/Shaan Body Milk/shaan-body-milk-300ml.jpg", ProductId = 21 },
          new Photo { Id = 22, ImageName = "images/Mood Shower Gel/mood-shower-gel-750ml.jpg", ProductId = 22 },
          new Photo { Id = 23, ImageName = "images/Skin Candy Oil/skin-candy-perfumed-hair-body-oil-50ml.jpg", ProductId = 23 },
          new Photo { Id = 24, ImageName = "images/Starville Roll On/starville-roll-on.jpg", ProductId = 24 },
          new Photo { Id = 25, ImageName = "images/Bodylicious Lotion/bodylicious-body-lotion-236ml.jpg", ProductId = 25 },
          new Photo { Id = 26, ImageName = "images/Watsons Body Mousse/watsons-body-mousse-75ml.jpg", ProductId = 26 },
          new Photo { Id = 27, ImageName = "images/Skin Candy Deodorant/skin-candy-deodorant-cream.png", ProductId = 27 }

          );

    }
  }
}
