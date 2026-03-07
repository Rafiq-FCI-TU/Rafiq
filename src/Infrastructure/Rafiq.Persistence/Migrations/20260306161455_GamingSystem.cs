using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GamingSystem : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_GameCategory_GamingId",
                table: "GameCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09c7b2bf-50db-47c7-be43-3c74ac332e26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa8017f7-0843-4230-87c1-9c2311f4b638");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Gaming");

            migrationBuilder.DropColumn(
                name: "AchievedPoints",
                table: "GameProgress");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "GameProgress");

            migrationBuilder.DropColumn(
                name: "GamingId",
                table: "GameCategory");

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

            migrationBuilder.RenameColumn(
                name: "TimePlayed",
                table: "GameProgresses",
                newName: "EarnedPoints");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "GameProgresses",
                newName: "GameLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgress_PatientId",
                table: "GameProgresses",
                newName: "IX_GameProgresses_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgress_GameId",
                table: "GameProgresses",
                newName: "IX_GameProgresses_GameLevelId");

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

            migrationBuilder.CreateTable(
                name: "GameLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    RequiredScore = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    RewardPoints = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameLevels_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "79825897-a3b8-4fa6-b45b-df2d9ae3dab2", "5a84e56e-e89c-4b51-b3ac-66c30ff9022b", "Specialist", "SPECIALIST" },
                    { "c41c40e8-3b08-410d-a4d8-002db008e48f", "1152f893-44b1-4e0f-a8db-87a98df0be83", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gamings_PatientId",
                table: "Gamings",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLevels_GameId",
                table: "GameLevels",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameProgresses_GameLevels_GameLevelId",
                table: "GameProgresses",
                column: "GameLevelId",
                principalTable: "GameLevels",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Gamings_Patients_PatientId",
                table: "Gamings",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProgresses_GameLevels_GameLevelId",
                table: "GameProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProgresses_Patients_PatientId",
                table: "GameProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameCategories_CategoryId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Gamings_Patients_PatientId",
                table: "Gamings");

            migrationBuilder.DropTable(
                name: "GameLevels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Gamings",
                table: "Gamings");

            migrationBuilder.DropIndex(
                name: "IX_Gamings_PatientId",
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
                keyValue: "79825897-a3b8-4fa6-b45b-df2d9ae3dab2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c41c40e8-3b08-410d-a4d8-002db008e48f");

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

            migrationBuilder.RenameColumn(
                name: "GameLevelId",
                table: "GameProgress",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "EarnedPoints",
                table: "GameProgress",
                newName: "TimePlayed");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgresses_PatientId",
                table: "GameProgress",
                newName: "IX_GameProgress_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_GameProgresses_GameLevelId",
                table: "GameProgress",
                newName: "IX_GameProgress_GameId");

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "Gaming",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AchievedPoints",
                table: "GameProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "GameProgress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GamingId",
                table: "GameCategory",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                    { "09c7b2bf-50db-47c7-be43-3c74ac332e26", "06f1757e-c895-4ac6-9b8e-167c5ec7a6ac", "Specialist", "SPECIALIST" },
                    { "fa8017f7-0843-4230-87c1-9c2311f4b638", "c4f86e56-66e0-4da3-a9a8-793c2f4e296f", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCategory_GamingId",
                table: "GameCategory",
                column: "GamingId");

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
