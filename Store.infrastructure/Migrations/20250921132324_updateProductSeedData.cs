using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5, 180m, 20, 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2000000000000002, 160m, 15, 40 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.7000000000000002, 180m, 30, 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.0, 100m, 12, 70 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2999999999999998, 140m, 18, 30 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.0999999999999996, 130m, 10, 25 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5999999999999996, 120m, 25, 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.4000000000000004, 150m, 40, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.7999999999999998, 230m, 35, 45 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5, 170m, 22, 55 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.0, 90m, 12, 90 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5999999999999996, 190m, 28, 35 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2999999999999998, 160m, 19, 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2000000000000002, 110m, 15, 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.4000000000000004, 130m, 23, 65 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2999999999999998, 120m, 20, 75 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2000000000000002, 115m, 18, 55 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.0999999999999996, 125m, 15, 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5, 200m, 26, 70 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5999999999999996, 170m, 30, 65 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2000000000000002, 140m, 18, 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2999999999999998, 160m, 22, 75 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.5999999999999996, 190m, 25, 40 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.0, 110m, 12, 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.4000000000000004, 170m, 20, 55 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.0999999999999996, 150m, 14, 45 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 4.2000000000000002, 130m, 16, 50 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "AverageRating", "OldPrice", "ReviewCount", "Stock" },
                values: new object[] { 0.0, 0m, 0, 0 });
        }
    }
}
