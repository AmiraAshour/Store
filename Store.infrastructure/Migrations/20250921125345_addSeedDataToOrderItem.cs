using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSeedDataToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "MainImage", "OrdersId", "Price", "ProductItemId", "ProductName", "Quntity" },
                values: new object[,]
                {
                    { 1, "images/Bio Soft Deep Conditioner/bio-soft-deep-conditioner-500g.jpg", null, 150m, 1, "Bio Soft Deep Conditioner", 2 },
                    { 2, "images/Bio Soft Shampoo/bio-soft-shampoo-500ml.jpg", null, 120m, 2, "Bio Soft Shampoo", 5 },
                    { 3, "images/Dermatique Sun/dermatique-sun-mattifying-fluid-50ml.jpg", null, 160m, 3, "Dermatique Sun Mattifying Fluid", 1 },
                    { 4, "images/Dermatique Hydrating/dermatique-hydrating-cream-100ml.jpg", null, 180m, 4, "Dermatique Hydrating Cream", 4 },
                    { 5, "images/LOreal/loreal-serum-50ml.jpg", null, 200m, 5, "L’Oreal Serum", 3 },
                    { 6, "images/LOreal/loreal-conditioner-250ml.jpg", null, 140m, 6, "L’Oreal Conditioner", 6 },
                    { 7, "images/The Ordinary/niacinamide-30ml.jpg", null, 220m, 7, "The Ordinary Niacinamide", 7 },
                    { 8, "images/The Ordinary/hyaluronic-30ml.jpg", null, 210m, 8, "The Ordinary Hyaluronic Acid", 2 },
                    { 9, "images/Olaplex/olaplex-no.3-hair-perfector-100ml.png", null, 200m, 9, "Olaplex No.3 Hair Perfector", 3 },
                    { 10, "images/Olaplex/olaplex-no.6-bond-smoother-100ml.png", null, 230m, 10, "Olaplex No.6 Bond Smoother", 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
