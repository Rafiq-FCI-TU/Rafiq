using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreColumnForAssessmentCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09c7b2bf-50db-47c7-be43-3c74ac332e26");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa8017f7-0843-4230-87c1-9c2311f4b638");

            migrationBuilder.AddColumn<double>(
                name: "score",
                table: "Questions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "AssessmentCategory",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "549ea74f-0c74-45d9-b0c5-57f29c0c6713", "aba5ee1c-63f1-4101-a010-10b02d3df9fe", "Family", "FAMILY" },
                    { "d12773ef-e14c-4ad7-81fa-f173f8eab3eb", "175c2807-2121-445d-9680-6b6d0479397e", "Specialist", "SPECIALIST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "549ea74f-0c74-45d9-b0c5-57f29c0c6713");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d12773ef-e14c-4ad7-81fa-f173f8eab3eb");

            migrationBuilder.DropColumn(
                name: "score",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "AssessmentCategory",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09c7b2bf-50db-47c7-be43-3c74ac332e26", "06f1757e-c895-4ac6-9b8e-167c5ec7a6ac", "Specialist", "SPECIALIST" },
                    { "fa8017f7-0843-4230-87c1-9c2311f4b638", "c4f86e56-66e0-4da3-a9a8-793c2f4e296f", "Family", "FAMILY" }
                });
        }
    }
}
