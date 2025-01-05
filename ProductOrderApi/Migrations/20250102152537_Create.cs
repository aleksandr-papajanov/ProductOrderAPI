using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductOrderApi.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.CheckConstraint("CK_Product_Price", "Price > 0");
                    table.CheckConstraint("CK_Product_QuantityInStock", "QuantityInStock >= 0");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductFeatures",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    FeatureId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatures", x => new { x.ProductId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_ProductFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductFeatures_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserConfirmationToken",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfirmationToken", x => new { x.UserId, x.Token });
                    table.ForeignKey(
                        name: "FK_UserConfirmationToken_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.Role });
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.OrderId, x.ProductId });
                    table.CheckConstraint("CK_OrderProduct_Price", "Price > 0");
                    table.CheckConstraint("CK_OrderProduct_Quantity", "Price > 0");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderTracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTracking", x => x.Id);
                    table.CheckConstraint("CK_OrderTracking_Status", "Status BETWEEN 1 AND 8");
                    table.ForeignKey(
                        name: "FK_OrderTracking_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Color" },
                    { 2, "Length" },
                    { 3, "Weight" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "IsAvailable", "ModifiedAt", "Name", "Price", "QuantityInStock" },
                values: new object[,]
                {
                    { 1, "AD268754", new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6608), "Test description for product 1", true, new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6646), "Product 1", 15.99m, 14 },
                    { 2, "FR235467", new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6650), "Test description for product 2", true, new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6651), "Product 2", 1.50m, 3 },
                    { 3, "TY547756", new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6653), "Test description for product 3", true, new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6654), "Product 3", 24.99m, 11 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Password" },
                values: new object[,]
                {
                    { 1, "admin@example.com", false, "fFied55ufW537BcXC4z0CHqWWZ7gWwyI6K5OgZVG32VP2tScM02Mv/BWLWSI7nVL" },
                    { 2, "customer@example.com", false, "fFied55ufW537BcXC4z0CHqWWZ7gWwyI6K5OgZVG32VP2tScM02Mv/BWLWSI7nVL" },
                    { 3, "admin.custumer@example.com", false, "fFied55ufW537BcXC4z0CHqWWZ7gWwyI6K5OgZVG32VP2tScM02Mv/BWLWSI7nVL" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Comment", "TotalPrice", "UserId" },
                values: new object[,]
                {
                    { 1, "Test order 1", 133.14m, 1 },
                    { 2, "Test order 2", 444.12m, 2 },
                    { 3, "Test order 3", 234.12m, 3 }
                });

            migrationBuilder.InsertData(
                table: "ProductFeatures",
                columns: new[] { "FeatureId", "ProductId", "Value" },
                values: new object[,]
                {
                    { 1, 1, "Red" },
                    { 2, 1, "50mm" },
                    { 3, 1, "12.455 kg" },
                    { 1, 2, "Greed" },
                    { 2, 2, "200mm" },
                    { 3, 2, "0.455 kg" },
                    { 1, 3, "Blue" },
                    { 2, 3, "1010mm" },
                    { 3, 3, "1.12 kg" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Role", "UserId" },
                values: new object[,]
                {
                    { "Admin", 1 },
                    { "Customer", 2 },
                    { "Admin", 3 },
                    { "Customer", 3 }
                });

            migrationBuilder.InsertData(
                table: "OrderProducts",
                columns: new[] { "OrderId", "ProductId", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 11.51m, 1 },
                    { 1, 2, 15.20m, 2 },
                    { 1, 3, 1234.19m, 3 },
                    { 2, 2, 1451.60m, 1 },
                    { 2, 3, 1321.12m, 2 },
                    { 3, 2, 151.42m, 14 }
                });

            migrationBuilder.InsertData(
                table: "OrderTracking",
                columns: new[] { "Id", "OrderId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2024, 12, 30, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7052) },
                    { 2, 1, 2, new DateTime(2024, 12, 31, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7058) },
                    { 3, 1, 3, new DateTime(2025, 1, 1, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7060) },
                    { 4, 2, 1, new DateTime(2024, 12, 28, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7061) },
                    { 5, 2, 2, new DateTime(2024, 12, 29, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7063) },
                    { 6, 2, 3, new DateTime(2024, 12, 30, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7064) },
                    { 7, 2, 6, new DateTime(2024, 12, 31, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7066) },
                    { 8, 3, 1, new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7067) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_Name",
                table: "Features",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTracking_OrderId",
                table: "OrderTracking",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_FeatureId",
                table: "ProductFeatures",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserConfirmationToken_UserId",
                table: "UserConfirmationToken",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "OrderTracking");

            migrationBuilder.DropTable(
                name: "ProductFeatures");

            migrationBuilder.DropTable(
                name: "UserConfirmationToken");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
