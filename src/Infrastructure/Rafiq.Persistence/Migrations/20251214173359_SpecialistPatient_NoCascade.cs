using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SpecialistPatient_NoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Patients_PatientId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Assessments_AssessmentId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialistPatients_AspNetUsers_SpecialistId",
                table: "SpecialistPatients");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "076216a9-0edd-467f-8f46-230af2af6164");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2bfa9ea-d553-4e04-a504-f4454cd95882");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Questions",
                newName: "SelectedAnswer");

            migrationBuilder.RenameColumn(
                name: "AssessmentId",
                table: "Questions",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_AssessmentId",
                table: "Questions",
                newName: "IX_Questions_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Assessments",
                newName: "TotalScore");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerType",
                table: "Questions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalScore",
                table: "Patients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CurrentCategory",
                table: "Assessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuestionsNumber",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 40);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedOption",
                table: "Answers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AssessmentCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    QuestionsNumber = table.Column<int>(type: "int", nullable: false),
                    CurrentQuestion = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentCategory_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gaming",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gaming", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialistProfileId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Session_AspNetUsers_SpecialistProfileId",
                        column: x => x.SpecialistProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GamingId = table.Column<int>(type: "int", nullable: false),
                    InitialScore = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCategory_Gaming_GamingId",
                        column: x => x.GamingId,
                        principalTable: "Gaming",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    MaxPoints = table.Column<int>(type: "int", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_GameCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GameCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameProgress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    AchievedPoints = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TimePlayed = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Completed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PlayedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameProgress_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameProgress_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4956601-975d-4a80-bb5b-43299f240cd7", "1b6e1131-5240-4a42-a4b4-ffe8737e69c6", "Specialist", "SPECIALIST" },
                    { "c1101165-b643-40f8-9542-481b81eadf73", "9ed2faae-119a-4749-a7dc-8e04cbfc5946", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentCategory_AssessmentId",
                table: "AssessmentCategory",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_CategoryId",
                table: "Game",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCategory_GamingId",
                table: "GameCategory",
                column: "GamingId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProgress_GameId",
                table: "GameProgress",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProgress_PatientId",
                table: "GameProgress",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_SpecialistProfileId",
                table: "Session",
                column: "SpecialistProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Patients_PatientId",
                table: "Assessments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AssessmentCategory_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "AssessmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialistPatients_AspNetUsers_SpecialistId",
                table: "SpecialistPatients",
                column: "SpecialistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Patients_PatientId",
                table: "Assessments");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AssessmentCategory_CategoryId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecialistPatients_AspNetUsers_SpecialistId",
                table: "SpecialistPatients");

            migrationBuilder.DropTable(
                name: "AssessmentCategory");

            migrationBuilder.DropTable(
                name: "GameProgress");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "GameCategory");

            migrationBuilder.DropTable(
                name: "Gaming");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4956601-975d-4a80-bb5b-43299f240cd7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1101165-b643-40f8-9542-481b81eadf73");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CurrentCategory",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "QuestionsNumber",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "SelectedAnswer",
                table: "Questions",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Questions",
                newName: "AssessmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_CategoryId",
                table: "Questions",
                newName: "IX_Questions_AssessmentId");

            migrationBuilder.RenameColumn(
                name: "TotalScore",
                table: "Assessments",
                newName: "Score");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerType",
                table: "Questions",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedOption",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "076216a9-0edd-467f-8f46-230af2af6164", "5ebd9457-0518-4e7c-b3ce-4e001224b727", "Specialist", "SPECIALIST" },
                    { "c2bfa9ea-d553-4e04-a504-f4454cd95882", "321921d3-d538-4824-848e-a1d1fbd20bea", "Family", "FAMILY" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Patients_PatientId",
                table: "Assessments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Assessments_AssessmentId",
                table: "Questions",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialistPatients_AspNetUsers_SpecialistId",
                table: "SpecialistPatients",
                column: "SpecialistId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
