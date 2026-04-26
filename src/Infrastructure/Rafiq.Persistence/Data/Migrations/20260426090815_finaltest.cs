using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class finaltest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameOption_GameQuestion_GameQuestionId",
                table: "GameOption");

            migrationBuilder.DropForeignKey(
                name: "FK_GameQuestion_GameLevels_GameLevelId",
                table: "GameQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameQuestion",
                table: "GameQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameOption",
                table: "GameOption");

            migrationBuilder.RenameTable(
                name: "GameQuestion",
                newName: "GameQuestions");

            migrationBuilder.RenameTable(
                name: "GameOption",
                newName: "GameOptions");

            migrationBuilder.RenameIndex(
                name: "IX_GameQuestion_GameLevelId",
                table: "GameQuestions",
                newName: "IX_GameQuestions_GameLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOption_GameQuestionId",
                table: "GameOptions",
                newName: "IX_GameOptions_GameQuestionId");

            migrationBuilder.AlterColumn<double>(
                name: "EarnedPoints",
                table: "PatientLevels",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "MaxPoints",
                table: "PatientLevels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "PatientLevels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameQuestions",
                table: "GameQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameOptions",
                table: "GameOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameOptions_GameQuestions_GameQuestionId",
                table: "GameOptions",
                column: "GameQuestionId",
                principalTable: "GameQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameQuestions_GameLevels_GameLevelId",
                table: "GameQuestions",
                column: "GameLevelId",
                principalTable: "GameLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameOptions_GameQuestions_GameQuestionId",
                table: "GameOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_GameQuestions_GameLevels_GameLevelId",
                table: "GameQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameQuestions",
                table: "GameQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameOptions",
                table: "GameOptions");

            migrationBuilder.DropColumn(
                name: "MaxPoints",
                table: "PatientLevels");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "PatientLevels");

            migrationBuilder.RenameTable(
                name: "GameQuestions",
                newName: "GameQuestion");

            migrationBuilder.RenameTable(
                name: "GameOptions",
                newName: "GameOption");

            migrationBuilder.RenameIndex(
                name: "IX_GameQuestions_GameLevelId",
                table: "GameQuestion",
                newName: "IX_GameQuestion_GameLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_GameOptions_GameQuestionId",
                table: "GameOption",
                newName: "IX_GameOption_GameQuestionId");

            migrationBuilder.AlterColumn<int>(
                name: "EarnedPoints",
                table: "PatientLevels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameQuestion",
                table: "GameQuestion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameOption",
                table: "GameOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameOption_GameQuestion_GameQuestionId",
                table: "GameOption",
                column: "GameQuestionId",
                principalTable: "GameQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameQuestion_GameLevels_GameLevelId",
                table: "GameQuestion",
                column: "GameLevelId",
                principalTable: "GameLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
