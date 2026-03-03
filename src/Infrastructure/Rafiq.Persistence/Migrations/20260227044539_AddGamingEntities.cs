using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGamingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_GameCategory_CategoryId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_GameCategory_Gaming_GamingId",
                table: "GameCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProgress_Game_GameId",
                table: "GameProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProgress_Patients_PatientId",
                table: "GameProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gaming",
                table: "Gaming");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameProgress",
                table: "GameProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCategory",
                table: "GameCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4956601-975d-4a80-bb5b-43299f240cd7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1101165-b643-40f8-9542-481b81eadf73");

            migrationBuilder.RenameTable(
                name: "Gaming",
                newName: "Gamings");

            migrationBuilder.RenameTable(
                name: "GameProgress",
                newName: "GameProgresses");

            migrationBuilder.RenameTable(
                name: "GameCategory",
                newName: "GameCategories");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgress_PatientId",
                table: "GameProgresses",
                newName: "IX_GameProgresses_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgress_GameId",
                table: "GameProgresses",
                newName: "IX_GameProgresses_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategory_GamingId",
                table: "GameCategories",
                newName: "IX_GameCategories_GamingId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_CategoryId",
                table: "Games",
                newName: "IX_Games_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gamings",
                table: "Gamings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameProgresses",
                table: "GameProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCategories",
                table: "GameCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20baec5c-7c6c-4d6b-a2d7-b6a60ecac4f5", "a7a658a1-0519-461d-bf28-845c0deee19d", "Family", "FAMILY" },
                    { "83035346-4239-4824-b4cc-f9dda0fe042f", "f96d90e4-ca1d-47af-9d62-0b13716f62d1", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategories_Gamings_GamingId",
                table: "GameCategories",
                column: "GamingId",
                principalTable: "Gamings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameProgresses_Games_GameId",
                table: "GameProgresses",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameProgresses_Patients_PatientId",
                table: "GameProgresses",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameCategories_CategoryId",
                table: "Games",
                column: "CategoryId",
                principalTable: "GameCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameCategories_Gamings_GamingId",
                table: "GameCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProgresses_Games_GameId",
                table: "GameProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProgresses_Patients_PatientId",
                table: "GameProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameCategories_CategoryId",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gamings",
                table: "Gamings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameProgresses",
                table: "GameProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameCategories",
                table: "GameCategories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20baec5c-7c6c-4d6b-a2d7-b6a60ecac4f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83035346-4239-4824-b4cc-f9dda0fe042f");

            migrationBuilder.RenameTable(
                name: "Gamings",
                newName: "Gaming");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.RenameTable(
                name: "GameProgresses",
                newName: "GameProgress");

            migrationBuilder.RenameTable(
                name: "GameCategories",
                newName: "GameCategory");

            migrationBuilder.RenameIndex(
                name: "IX_Games_CategoryId",
                table: "Game",
                newName: "IX_Game_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgresses_PatientId",
                table: "GameProgress",
                newName: "IX_GameProgress_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgresses_GameId",
                table: "GameProgress",
                newName: "IX_GameProgress_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_GameCategories_GamingId",
                table: "GameCategory",
                newName: "IX_GameCategory_GamingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Gaming",
                table: "Gaming",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameProgress",
                table: "GameProgress",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameCategory",
                table: "GameCategory",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4956601-975d-4a80-bb5b-43299f240cd7", "1b6e1131-5240-4a42-a4b4-ffe8737e69c6", "Specialist", "SPECIALIST" },
                    { "c1101165-b643-40f8-9542-481b81eadf73", "9ed2faae-119a-4749-a7dc-8e04cbfc5946", "Family", "FAMILY" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Game_GameCategory_CategoryId",
                table: "Game",
                column: "CategoryId",
                principalTable: "GameCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameCategory_Gaming_GamingId",
                table: "GameCategory",
                column: "GamingId",
                principalTable: "Gaming",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameProgress_Game_GameId",
                table: "GameProgress",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameProgress_Patients_PatientId",
                table: "GameProgress",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
