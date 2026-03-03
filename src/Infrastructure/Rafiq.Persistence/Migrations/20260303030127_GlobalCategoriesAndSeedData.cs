using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GlobalCategoriesAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCategories_Gamings_GamingId",
                table: "GameCategories");

            migrationBuilder.DropIndex(
                name: "IX_GameCategories_GamingId",
                table: "GameCategories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20baec5c-7c6c-4d6b-a2d7-b6a60ecac4f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83035346-4239-4824-b4cc-f9dda0fe042f");

            migrationBuilder.DropColumn(
                name: "GamingId",
                table: "GameCategories");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5999cb1e-104e-4580-b296-76770fdc5614", "e0b231ba-f1d6-4bfb-b3b0-dea52635d8a1", "Family", "FAMILY" },
                    { "a18a2e2f-7275-4237-9c5f-cf6647728bfb", "6bb51057-b59a-40e8-bf2c-797cd4ec7905", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.InsertData(
                table: "GameCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cognitive Skills" },
                    { 2, "Memory Training" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Duration", "MaxPoints", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 3, 3, 5, 1, 27, 327, DateTimeKind.Local).AddTicks(4852), "5 mins", 100, 0, "Match Shapes" },
                    { 2, 1, new DateTime(2026, 3, 3, 5, 1, 27, 327, DateTimeKind.Local).AddTicks(4907), "10 mins", 200, 0, "Logic Puzzle" },
                    { 3, 2, new DateTime(2026, 3, 3, 5, 1, 27, 327, DateTimeKind.Local).AddTicks(4910), "8 mins", 150, 0, "Card Flip" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5999cb1e-104e-4580-b296-76770fdc5614");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18a2e2f-7275-4237-9c5f-cf6647728bfb");

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Games",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "GamingId",
                table: "GameCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20baec5c-7c6c-4d6b-a2d7-b6a60ecac4f5", "a7a658a1-0519-461d-bf28-845c0deee19d", "Family", "FAMILY" },
                    { "83035346-4239-4824-b4cc-f9dda0fe042f", "f96d90e4-ca1d-47af-9d62-0b13716f62d1", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCategories_GamingId",
                table: "GameCategories",
                column: "GamingId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategories_Gamings_GamingId",
                table: "GameCategories",
                column: "GamingId",
                principalTable: "Gamings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
