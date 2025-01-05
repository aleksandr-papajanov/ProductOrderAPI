using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductOrderApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleAuthState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGoogleUser",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GoogleAuthStates",
                columns: table => new
                {
                    State = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RequestType = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleAuthStates", x => x.State);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 1, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6772));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 2, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6778));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 3, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 30, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6781));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 31, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6782));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 1, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6784));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 2, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6786));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6787));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6307), new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6360) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6364), new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6365) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6367), new DateTime(2025, 1, 4, 16, 53, 25, 742, DateTimeKind.Local).AddTicks(6368) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsGoogleUser",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsGoogleUser",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsGoogleUser",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleAuthStates");

            migrationBuilder.DropColumn(
                name: "IsGoogleUser",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 31, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1246));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 1, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1251));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 2, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1253));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 29, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1255));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 30, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1256));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 31, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1258));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 1, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1259));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(1261));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(650), new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(695) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(699), new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(700) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(703), new DateTime(2025, 1, 3, 19, 19, 10, 407, DateTimeKind.Local).AddTicks(704) });
        }
    }
}
