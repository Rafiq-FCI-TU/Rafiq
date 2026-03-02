using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "055759ea-3633-4096-ac64-9e8fcc1c6458");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5119c721-1596-4bce-bf4e-577b1d66925a");

            migrationBuilder.CreateTable(
                name: "PasswordResetToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Otp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetToken_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "72e29afe-8e79-4290-8868-5c13a57dd61f", "e2c9ffee-290a-4327-801d-8bd2e452d02e", "Family", "FAMILY" },
                    { "768ac1ca-c0f9-4b46-89c7-e01ef12746ab", "76cd1a43-e2a9-46f1-91e1-f0f71824616a", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetToken_ApplicationUserId",
                table: "PasswordResetToken",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetToken");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72e29afe-8e79-4290-8868-5c13a57dd61f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "768ac1ca-c0f9-4b46-89c7-e01ef12746ab");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "055759ea-3633-4096-ac64-9e8fcc1c6458", "9cc7575a-9caa-4ef6-a2ec-a3915be36c64", "Specialist", "SPECIALIST" },
                    { "5119c721-1596-4bce-bf4e-577b1d66925a", "fe81c00e-d9be-435e-b5fc-3e793f77063b", "Family", "FAMILY" }
                });
        }
    }
}
