using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_deliveryMethods_deliveryMethodId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_deliveryMethods",
                table: "deliveryMethods");

            migrationBuilder.RenameTable(
                name: "deliveryMethods",
                newName: "DeliveryMethods");

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryMethods",
                table: "DeliveryMethods",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BuyerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AverageRating", "ReviewCount" },
                values: new object[] { 0.0, 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AverageRating", "ReviewCount" },
                values: new object[] { 0.0, 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_deliveryMethodId",
                table: "Orders",
                column: "deliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_deliveryMethodId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryMethods",
                table: "DeliveryMethods");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "DeliveryMethods",
                newName: "deliveryMethods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_deliveryMethods",
                table: "deliveryMethods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_deliveryMethods_deliveryMethodId",
                table: "Orders",
                column: "deliveryMethodId",
                principalTable: "deliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
