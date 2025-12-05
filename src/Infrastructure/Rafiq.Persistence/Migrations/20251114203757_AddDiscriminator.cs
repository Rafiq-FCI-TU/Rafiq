using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Assessments_AssessmentId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_AssessmentId_QuestionId",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2c78a36-8f6c-469b-8e87-a67f9abdfa19");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cda82048-524f-4d92-a3ae-491a032adb42");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "AnsweredAt",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "DifficultyAnswer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "FrequencyAnswer",
                table: "Answers");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerType",
                table: "Questions",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "SelectedOption",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "076216a9-0edd-467f-8f46-230af2af6164", "5ebd9457-0518-4e7c-b3ce-4e001224b727", "Specialist", "SPECIALIST" },
                    { "c2bfa9ea-d553-4e04-a504-f4454cd95882", "321921d3-d538-4824-848e-a1d1fbd20bea", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AssessmentId",
                table: "Questions",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Assessments_AssessmentId",
                table: "Questions",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Assessments_AssessmentId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AssessmentId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "076216a9-0edd-467f-8f46-230af2af6164");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2bfa9ea-d553-4e04-a504-f4454cd95882");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "SelectedOption",
                table: "Answers");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerType",
                table: "Questions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Assessments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweredAt",
                table: "Answers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Answers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DifficultyAnswer",
                table: "Answers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrequencyAnswer",
                table: "Answers",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b2c78a36-8f6c-469b-8e87-a67f9abdfa19", "ca1d6681-a431-4f64-bcd8-de4be5d5257c", "Family", "FAMILY" },
                    { "cda82048-524f-4d92-a3ae-491a032adb42", "5ff2f076-b6bf-4ddb-b9f9-67b8ed6dd352", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_AssessmentId_QuestionId",
                table: "Answers",
                columns: new[] { "AssessmentId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Assessments_AssessmentId",
                table: "Answers",
                column: "AssessmentId",
                principalTable: "Assessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
