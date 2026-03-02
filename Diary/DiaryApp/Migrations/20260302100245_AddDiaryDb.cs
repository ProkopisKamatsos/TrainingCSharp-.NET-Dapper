using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DiaryApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDiaryDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DiaryEntries",
                columns: new[] { "Id", "Content", "Created", "Title" },
                values: new object[,]
                {
                    { 1, "Hiking with Nunn", new DateTime(2026, 3, 2, 12, 2, 43, 15, DateTimeKind.Local).AddTicks(4004), "Hiking" },
                    { 2, "Shopping with Nunn", new DateTime(2026, 3, 2, 12, 2, 43, 15, DateTimeKind.Local).AddTicks(5311), "Shopping" },
                    { 3, "Diving with Nunn", new DateTime(2026, 3, 2, 12, 2, 43, 15, DateTimeKind.Local).AddTicks(5319), "Diving" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DiaryEntries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DiaryEntries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DiaryEntries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
