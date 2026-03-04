using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class assessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AssessmentCategory_CategoryId",
                table: "Questions");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72e29afe-8e79-4290-8868-5c13a57dd61f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "768ac1ca-c0f9-4b46-89c7-e01ef12746ab");

            migrationBuilder.DropColumn(
                name: "SelectedAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CurrentCategory",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "QuestionsNumber",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Assessments");

            migrationBuilder.RenameColumn(
                name: "Sequence",
                table: "AssessmentCategory",
                newName: "CategoryId");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a586159d-5080-4731-96a6-3d3b60384538", "e9e1913c-3c73-41da-a2ff-8272d60c7a1a", "Specialist", "SPECIALIST" },
                    { "ed24e09b-02e6-4a82-b211-4cc4d0e7af79", "89ed058d-f96f-41a8-b8f4-25c747f29c5c", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentCategory_CategoryId",
                table: "AssessmentCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentCategory_Category_CategoryId",
                table: "AssessmentCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Category_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentCategory_Category_CategoryId",
                table: "AssessmentCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Category_CategoryId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentCategory_CategoryId",
                table: "AssessmentCategory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a586159d-5080-4731-96a6-3d3b60384538");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed24e09b-02e6-4a82-b211-4cc4d0e7af79");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "AssessmentCategory",
                newName: "Sequence");

            migrationBuilder.AddColumn<string>(
                name: "SelectedAnswer",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "72e29afe-8e79-4290-8868-5c13a57dd61f", "e2c9ffee-290a-4327-801d-8bd2e452d02e", "Family", "FAMILY" },
                    { "768ac1ca-c0f9-4b46-89c7-e01ef12746ab", "76cd1a43-e2a9-46f1-91e1-f0f71824616a", "Specialist", "SPECIALIST" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AssessmentCategory_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "AssessmentCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
