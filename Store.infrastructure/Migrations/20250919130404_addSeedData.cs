using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Skin care products", "Skin Care" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Hair care products", "Hair Care" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 3, "Makeup and cosmetics", "Makeup" },
                    { 4, "Body care products", "Body Care" }
                });

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageName",
                value: "images/Bio Soft Deep Conditioner/bio-soft-deep-conditioner-500g.jpg");

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageName",
                value: "images/Bobai Sun Screen/bobai-extra-lightening-sun-screen-gel-50gm.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Description", "Name", "NewPrice" },
                values: new object[] { 2, "Deep nourishing conditioner 500g", "Bio Soft Deep Conditioner", 150m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Name", "NewPrice" },
                values: new object[] { 1, "Extra lightening sunscreen gel 50gm", "Bobai Extra Lightening Sun Screen", 140m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AverageRating", "CategoryId", "Description", "Name", "NewPrice", "OldPrice", "ReviewCount", "Stock" },
                values: new object[,]
                {
                    { 3, 0.0, 1, "Mattifying sun fluid 50ml", "Dermatique Sun Mattifying Fluid", 160m, 0m, 0, 0 },
                    { 5, 0.0, 1, "Truly skin serum 35ml", "Lebelage Truly Serum", 120m, 0m, 0, 0 },
                    { 6, 0.0, 1, "Eye contour gel 15ml", "Leylak Eye Contour Gel", 110m, 0m, 0, 0 },
                    { 8, 0.0, 1, "Moisturizing cream 100g", "Moist 1 Cream", 130m, 0m, 0, 0 },
                    { 9, 0.0, 2, "Hair perfector 100ml", "Olaplex No.3 Hair Perfector", 200m, 0m, 0, 0 },
                    { 10, 0.0, 2, "Argan oil spray", "ORS Argan Oil Spray", 150m, 0m, 0, 0 },
                    { 11, 0.0, 1, "Black bubble mask", "Purederm Black Bubble Mask", 70m, 0m, 0, 0 },
                    { 12, 0.0, 2, "Follicle booster oil 100ml", "Raw African Follicle Booster Oil", 160m, 0m, 0, 0 },
                    { 13, 0.0, 2, "Shampoo 300ml", "Seropipe Hair Shampoo", 140m, 0m, 0, 0 },
                    { 14, 0.0, 1, "Make-up remover 200ml", "Shaan Make Up Remover", 90m, 0m, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "ImageName", "ProductId" },
                values: new object[,]
                {
                    { 3, "images/Dermatique Sun/dermatique-sun-mattifying-fluid-50ml.jpg", 3 },
                    { 5, "images/Lebelage Serum/lebelage-truly-serum-35ml.jpg", 5 },
                    { 6, "images/Leylak Eye Gel/leylak-eye-contour-gel-15ml.jpg", 6 },
                    { 8, "images/Moist 1 Cream/moist-1-cream-moisturizing-cream-100g.jpg", 8 },
                    { 9, "images/Olaplex/olaplex-no.3-hair-perfector-100ml.png", 9 },
                    { 10, "images/ORS Argan Oil/ors-argan-oil-spray.png", 10 },
                    { 11, "images/Purederm Mask/purederm-black-bubble-mask.png", 11 },
                    { 12, "images/Raw African Oil/raw-african-follicle-booster-oil-100ml.jpg", 12 },
                    { 13, "images/Seropipe Shampoo/seropipe-hair-shampoo-300ml.jpg", 13 },
                    { 14, "images/Shaan Remover/shaan-make-up-remover-200ml-600x600.jpg", 14 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AverageRating", "CategoryId", "Description", "Name", "NewPrice", "OldPrice", "ReviewCount", "Stock" },
                values: new object[,]
                {
                    { 4, 0.0, 3, "Tinted lip cheek balm", "Essence Juicy Melon Lip Balm", 80m, 0m, 0, 0 },
                    { 7, 0.0, 3, "Magic retouch 75ml", "Loreal Brown Magic Retouch", 100m, 0m, 0, 0 },
                    { 15, 0.0, 3, "Boost concealer Acorn", "Sheglam Complexion Boost Concealer", 110m, 0m, 0, 0 },
                    { 16, 0.0, 3, "Hydrating lip blush tint", "Sheglam Jelly Licious Lip Blush", 100m, 0m, 0, 0 },
                    { 17, 0.0, 3, "Liquid blush Petal Talk", "Sheglam Liquid Blush Petal Talk", 95m, 0m, 0, 0 },
                    { 18, 0.0, 3, "Setting powder duo Bisque", "Sheglam Setting Powder Duo", 105m, 0m, 0, 0 },
                    { 19, 0.0, 3, "Photo focus foundation soft ivory", "Wet n Wild Foundation", 170m, 0m, 0, 0 },
                    { 20, 0.0, 3, "Concealer Sand 20", "Maybelline Fit Me Concealer", 150m, 0m, 0, 0 },
                    { 21, 0.0, 4, "Body milk 300ml", "Shaan Body Milk", 120m, 0m, 0, 0 },
                    { 22, 0.0, 4, "Shower gel 750ml", "Mood Shower Gel", 140m, 0m, 0, 0 },
                    { 23, 0.0, 4, "Perfumed hair & body oil 50ml", "Skin Candy Perfumed Hair Body Oil", 160m, 0m, 0, 0 },
                    { 24, 0.0, 4, "Roll-on deodorant", "Starville Roll On", 90m, 0m, 0, 0 },
                    { 25, 0.0, 4, "Body lotion 236ml", "Bodylicious Body Lotion", 150m, 0m, 0, 0 },
                    { 26, 0.0, 4, "Body mousse 75ml", "Watsons Body Mousse", 130m, 0m, 0, 0 },
                    { 27, 0.0, 4, "Deodorant cream", "Skin Candy Deodorant Cream", 110m, 0m, 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "ImageName", "ProductId" },
                values: new object[,]
                {
                    { 4, "images/Essence Lip Balm/essence-juicy-melon-tinted-lip-cheek-balm.jpg", 4 },
                    { 7, "images/Loreal Retouch/loreal-brown-magic-retouch-75ml.png", 7 },
                    { 15, "images/Sheglam Concealer/sheglam-complexation-boost-concealer-acorn.jpg", 15 },
                    { 16, "images/Sheglam Lip Blush/sheglam-jelly-licious-hydrating-lip-blush-tint-aho.jpg", 16 },
                    { 17, "images/Sheglam Liquid Blush/sheglam-liquid-blush-petal-talk.jpg", 17 },
                    { 18, "images/Sheglam Powder/sheglam-setting-powder-duo-bisque-600x601.jpg", 18 },
                    { 19, "images/Wet n Wild/wet-n-wild-photofocus-foundation-362-soft-ivory.jpg", 19 },
                    { 20, "images/Maybelline Concealer/maybelline-fit-me-concealer-20-sand.jpg", 20 },
                    { 21, "images/Shaan Body Milk/shaan-body-milk-300ml.jpg", 21 },
                    { 22, "images/Mood Shower Gel/mood-shower-gel-750ml.jpg", 22 },
                    { 23, "images/Skin Candy Oil/skin-candy-perfumed-hair-body-oil-50ml.jpg", 23 },
                    { 24, "images/Starville Roll On/starville-roll-on.jpg", 24 },
                    { 25, "images/Bodylicious Lotion/bodylicious-body-lotion-236ml.jpg", 25 },
                    { 26, "images/Watsons Body Mousse/watsons-body-mousse-75ml.jpg", 26 },
                    { 27, "images/Skin Candy Deodorant/skin-candy-deodorant-cream.png", 27 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Electronic laptops", "Laptops" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Smart mobile phones", "Phones" });

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageName",
                value: "laptop1.jpg");

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageName",
                value: "phone1.jpg");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Description", "Name", "NewPrice" },
                values: new object[] { 1, "Gaming laptop", "Laptop", 25000m });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Name", "NewPrice" },
                values: new object[] { 2, "Fiction book", "Novel", 150m });
        }
    }
}
