using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddgemdercolumnsinSpecialisttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4956601-975d-4a80-bb5b-43299f240cd7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1101165-b643-40f8-9542-481b81eadf73");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "055759ea-3633-4096-ac64-9e8fcc1c6458", "9cc7575a-9caa-4ef6-a2ec-a3915be36c64", "Specialist", "SPECIALIST" },
                    { "5119c721-1596-4bce-bf4e-577b1d66925a", "fe81c00e-d9be-435e-b5fc-3e793f77063b", "Family", "FAMILY" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "055759ea-3633-4096-ac64-9e8fcc1c6458");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5119c721-1596-4bce-bf4e-577b1d66925a");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4956601-975d-4a80-bb5b-43299f240cd7", "1b6e1131-5240-4a42-a4b4-ffe8737e69c6", "Specialist", "SPECIALIST" },
                    { "c1101165-b643-40f8-9542-481b81eadf73", "9ed2faae-119a-4749-a7dc-8e04cbfc5946", "Family", "FAMILY" }
                });
        }
    }
}
