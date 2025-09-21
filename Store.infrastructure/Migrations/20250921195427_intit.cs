using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    DispalyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethods", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUser_Address",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    OldPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shippingAddress_FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shippingAddress_LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shippingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shippingAddress_ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shippingAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shippingAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeliveryMethodId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                        column: x => x.DeliveryMethodId,
                        principalTable: "DeliveryMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => new { x.ProductId, x.UserId });
                    table.ForeignKey(
                        name: "FK_WishlistItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductItemId = table.Column<int>(type: "int", nullable: false),
                    MainImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quntity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductItemId",
                        column: x => x.ProductItemId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Skin care products", "Skin Care" },
                    { 2, "Hair care products", "Hair Care" },
                    { 3, "Makeup and cosmetics", "Makeup" },
                    { 4, "Body care products", "Body Care" }
                });

            migrationBuilder.InsertData(
                table: "DeliveryMethods",
                columns: new[] { "Id", "DeliveryTime", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "1-2 days", "Fast delivery", "Fast", 10m },
                    { 2, "5-7 days", "Economy delivery", "Slow", 5m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AverageRating", "CategoryId", "Description", "Name", "NewPrice", "OldPrice", "ReviewCount", "Stock" },
                values: new object[,]
                {
                    { 1, 4.5, 2, "Deep nourishing conditioner 500g", "Bio Soft Deep Conditioner", 150m, 180m, 20, 50 },
                    { 2, 4.2000000000000002, 1, "Extra lightening sunscreen gel 50gm", "Bobai Extra Lightening Sun Screen", 140m, 160m, 15, 40 },
                    { 3, 4.7000000000000002, 1, "Mattifying sun fluid 50ml", "Dermatique Sun Mattifying Fluid", 160m, 180m, 30, 60 },
                    { 4, 4.0, 3, "Tinted lip cheek balm", "Essence Juicy Melon Lip Balm", 80m, 100m, 12, 70 },
                    { 5, 4.2999999999999998, 1, "Truly skin serum 35ml", "Lebelage Truly Serum", 120m, 140m, 18, 30 },
                    { 6, 4.0999999999999996, 1, "Eye contour gel 15ml", "Leylak Eye Contour Gel", 110m, 130m, 10, 25 },
                    { 7, 4.5999999999999996, 3, "Magic retouch 75ml", "Loreal Brown Magic Retouch", 100m, 120m, 25, 80 },
                    { 8, 4.4000000000000004, 1, "Moisturizing cream 100g", "Moist 1 Cream", 130m, 150m, 40, 100 },
                    { 9, 4.7999999999999998, 2, "Hair perfector 100ml", "Olaplex No.3 Hair Perfector", 200m, 230m, 35, 45 },
                    { 10, 4.5, 2, "Argan oil spray", "ORS Argan Oil Spray", 150m, 170m, 22, 55 },
                    { 11, 4.0, 1, "Black bubble mask", "Purederm Black Bubble Mask", 70m, 90m, 12, 90 },
                    { 12, 4.5999999999999996, 2, "Follicle booster oil 100ml", "Raw African Follicle Booster Oil", 160m, 190m, 28, 35 },
                    { 13, 4.2999999999999998, 2, "Shampoo 300ml", "Seropipe Hair Shampoo", 140m, 160m, 19, 60 },
                    { 14, 4.2000000000000002, 1, "Make-up remover 200ml", "Shaan Make Up Remover", 90m, 110m, 15, 50 },
                    { 15, 4.4000000000000004, 3, "Boost concealer Acorn", "Sheglam Complexion Boost Concealer", 110m, 130m, 23, 65 },
                    { 16, 4.2999999999999998, 3, "Hydrating lip blush tint", "Sheglam Jelly Licious Lip Blush", 100m, 120m, 20, 75 },
                    { 17, 4.2000000000000002, 3, "Liquid blush Petal Talk", "Sheglam Liquid Blush Petal Talk", 95m, 115m, 18, 55 },
                    { 18, 4.0999999999999996, 3, "Setting powder duo Bisque", "Sheglam Setting Powder Duo", 105m, 125m, 15, 50 },
                    { 19, 4.5, 3, "Photo focus foundation soft ivory", "Wet n Wild Foundation", 170m, 200m, 26, 70 },
                    { 20, 4.5999999999999996, 3, "Concealer Sand 20", "Maybelline Fit Me Concealer", 150m, 170m, 30, 65 },
                    { 21, 4.2000000000000002, 4, "Body milk 300ml", "Shaan Body Milk", 120m, 140m, 18, 60 },
                    { 22, 4.2999999999999998, 4, "Shower gel 750ml", "Mood Shower Gel", 140m, 160m, 22, 75 },
                    { 23, 4.5999999999999996, 4, "Perfumed hair & body oil 50ml", "Skin Candy Perfumed Hair Body Oil", 160m, 190m, 25, 40 },
                    { 24, 4.0, 4, "Roll-on deodorant", "Starville Roll On", 90m, 110m, 12, 100 },
                    { 25, 4.4000000000000004, 4, "Body lotion 236ml", "Bodylicious Body Lotion", 150m, 170m, 20, 55 },
                    { 26, 4.0999999999999996, 4, "Body mousse 75ml", "Watsons Body Mousse", 130m, 150m, 14, 45 },
                    { 27, 4.2000000000000002, 4, "Deodorant cream", "Skin Candy Deodorant Cream", 110m, 130m, 16, 50 }
                });

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "ImageName", "ProductId" },
                values: new object[,]
                {
                    { 1, "images/Bio Soft Deep Conditioner/bio-soft-deep-conditioner-500g.jpg", 1 },
                    { 2, "images/Bobai Sun Screen/bobai-extra-lightening-sun-screen-gel-50gm.jpg", 2 },
                    { 3, "images/Dermatique Sun/dermatique-sun-mattifying-fluid-50ml.jpg", 3 },
                    { 4, "images/Essence Lip Balm/essence-juicy-melon-tinted-lip-cheek-balm.jpg", 4 },
                    { 5, "images/Lebelage Serum/lebelage-truly-serum-35ml.jpg", 5 },
                    { 6, "images/Leylak Eye Gel/leylak-eye-contour-gel-15ml.jpg", 6 },
                    { 7, "images/Loreal Retouch/loreal-brown-magic-retouch-75ml.png", 7 },
                    { 8, "images/Moist 1 Cream/moist-1-cream-moisturizing-cream-100g.jpg", 8 },
                    { 9, "images/Olaplex/olaplex-no.3-hair-perfector-100ml.png", 9 },
                    { 10, "images/ORS Argan Oil/ors-argan-oil-spray.png", 10 },
                    { 11, "images/Purederm Mask/purederm-black-bubble-mask.png", 11 },
                    { 12, "images/Raw African Oil/raw-african-follicle-booster-oil-100ml.jpg", 12 },
                    { 13, "images/Seropipe Shampoo/seropipe-hair-shampoo-300ml.jpg", 13 },
                    { 14, "images/Shaan Remover/shaan-make-up-remover-200ml-600x600.jpg", 14 },
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

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AppUserId",
                table: "Addresses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductItemId",
                table: "OrderItems",
                column: "ProductItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_UserId",
                table: "WishlistItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
