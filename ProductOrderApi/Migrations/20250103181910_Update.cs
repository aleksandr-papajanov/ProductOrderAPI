using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductOrderApi.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 1,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 30, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 2,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 31, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 3,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 1, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 4,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 28, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7061));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 5,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 29, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 6,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 30, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 7,
                column: "UpdatedAt",
                value: new DateTime(2024, 12, 31, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "OrderTracking",
                keyColumn: "Id",
                keyValue: 8,
                column: "UpdatedAt",
                value: new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(7067));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6608), new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6646) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6650), new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6651) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6653), new DateTime(2025, 1, 2, 16, 25, 37, 50, DateTimeKind.Local).AddTicks(6654) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsActive",
                value: false);
        }
    }
}
