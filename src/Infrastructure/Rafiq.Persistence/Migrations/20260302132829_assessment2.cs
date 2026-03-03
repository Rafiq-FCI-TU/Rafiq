using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class assessment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentCategory_Category_CategoryId",
                table: "AssessmentCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Category_CategoryId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a586159d-5080-4731-96a6-3d3b60384538");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed24e09b-02e6-4a82-b211-4cc4d0e7af79");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6292893f-b0fb-4b8f-aa4c-0115a6c8e186", "73b65606-1f27-44bb-b8d1-1ac6558b0eb6", "Specialist", "SPECIALIST" },
                    { "a03fc5f2-5a4d-4fe3-8931-234cef93c232", "44fb8c54-ea54-46f4-a767-0e5fe6609092", "Family", "FAMILY" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentCategory_Categories_CategoryId",
                table: "AssessmentCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Categories_CategoryId",
                table: "Questions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentCategory_Categories_CategoryId",
                table: "AssessmentCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Categories_CategoryId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6292893f-b0fb-4b8f-aa4c-0115a6c8e186");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a03fc5f2-5a4d-4fe3-8931-234cef93c232");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a586159d-5080-4731-96a6-3d3b60384538", "e9e1913c-3c73-41da-a2ff-8272d60c7a1a", "Specialist", "SPECIALIST" },
                    { "ed24e09b-02e6-4a82-b211-4cc4d0e7af79", "89ed058d-f96f-41a8-b8f4-25c747f29c5c", "Family", "FAMILY" }
                });

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
    }
}
