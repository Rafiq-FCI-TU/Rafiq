using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AssessmentAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6292893f-b0fb-4b8f-aa4c-0115a6c8e186");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a03fc5f2-5a4d-4fe3-8931-234cef93c232");

            migrationBuilder.CreateTable(
                name: "AssessmentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AnswerValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentAnswers_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09c7b2bf-50db-47c7-be43-3c74ac332e26", "06f1757e-c895-4ac6-9b8e-167c5ec7a6ac", "Specialist", "SPECIALIST" },
                    { "fa8017f7-0843-4230-87c1-9c2311f4b638", "c4f86e56-66e0-4da3-a9a8-793c2f4e296f", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAnswers_AssessmentId",
                table: "AssessmentAnswers",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentAnswers_QuestionId",
                table: "AssessmentAnswers",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentAnswers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09c7b2bf-50db-47c7-be43-3c74ac332e26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa8017f7-0843-4230-87c1-9c2311f4b638");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6292893f-b0fb-4b8f-aa4c-0115a6c8e186", "73b65606-1f27-44bb-b8d1-1ac6558b0eb6", "Specialist", "SPECIALIST" },
                    { "a03fc5f2-5a4d-4fe3-8931-234cef93c232", "44fb8c54-ea54-46f4-a767-0e5fe6609092", "Family", "FAMILY" }
                });
        }
    }
}
