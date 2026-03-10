using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangePatientTotalScoreFieldFromDecimalToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameProgresses");

            migrationBuilder.DropTable(
                name: "Gamings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e05496a-fe56-4626-8996-1f8cf23908c3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af135806-4736-4492-a734-07d56f12c272");

            migrationBuilder.DropColumn(
                name: "AnswerType",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RewardPoints",
                table: "GameLevels");

            migrationBuilder.DropColumn(
                name: "InitialScore",
                table: "GameCategories");

            migrationBuilder.AlterColumn<double>(
                name: "TotalScore",
                table: "Patients",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "GameLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GameCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PatientLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    GameLevelId = table.Column<int>(type: "int", nullable: false),
                    EarnedPoints = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLevels_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameLevelPatientLevel",
                columns: table => new
                {
                    GameLevelId = table.Column<int>(type: "int", nullable: false),
                    PatientLevelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLevelPatientLevel", x => new { x.GameLevelId, x.PatientLevelsId });
                    table.ForeignKey(
                        name: "FK_GameLevelPatientLevel_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLevelPatientLevel_PatientLevels_PatientLevelsId",
                        column: x => x.PatientLevelsId,
                        principalTable: "PatientLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameLevelPatientLevel_PatientLevelsId",
                table: "GameLevelPatientLevel",
                column: "PatientLevelsId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLevels_PatientId",
                table: "PatientLevels",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLevelPatientLevel");

            migrationBuilder.DropTable(
                name: "PatientLevels");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "GameLevels");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GameCategories");

            migrationBuilder.AddColumn<string>(
                name: "AnswerType",
                table: "Questions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "Patients",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Games",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MaxPoints",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RewardPoints",
                table: "GameLevels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitialScore",
                table: "GameCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GameProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameLevelId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    EarnedPoints = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameProgresses_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameProgresses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Gamings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gamings_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3e05496a-fe56-4626-8996-1f8cf23908c3", "5bbfb59d-77f4-460f-bf17-1f0e16c9b91a", "Specialist", "SPECIALIST" },
                    { "af135806-4736-4492-a734-07d56f12c272", "0bc71d80-2c8c-4991-95cf-f95664c910ae", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameProgresses_GameLevelId",
                table: "GameProgresses",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProgresses_PatientId",
                table: "GameProgresses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Gamings_PatientId",
                table: "Gamings",
                column: "PatientId");
        }
    }
}
