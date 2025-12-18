using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixSpecialistPatientRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialistPatients_Patients_PatientId",
                table: "SpecialistPatients");

            migrationBuilder.DropIndex(
                name: "IX_SpecialistPatients_PatientId",
                table: "SpecialistPatients");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88bbf859-1e82-4f6f-b7f3-275e2a3adcb2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb979ce2-9550-4c83-8c0e-d048ac87f067");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34262ac0-2c66-4a99-bc38-15ba9d39c9f5", "fa912364-5329-4aa3-815e-b7c2dfcc8879", "Specialist", "SPECIALIST" },
                    { "ac34b183-0958-4b8f-8e92-a85a749ec736", "38b459a8-85e5-4835-9a21-4d8bcc098f90", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialistPatients_PatientId",
                table: "SpecialistPatients",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialistPatients_Patients_PatientId",
                table: "SpecialistPatients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecialistPatients_Patients_PatientId",
                table: "SpecialistPatients");

            migrationBuilder.DropIndex(
                name: "IX_SpecialistPatients_PatientId",
                table: "SpecialistPatients");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34262ac0-2c66-4a99-bc38-15ba9d39c9f5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac34b183-0958-4b8f-8e92-a85a749ec736");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88bbf859-1e82-4f6f-b7f3-275e2a3adcb2", "9fc7e6ac-1257-4d65-a23f-c4121efbdb0e", "Specialist", "SPECIALIST" },
                    { "fb979ce2-9550-4c83-8c0e-d048ac87f067", "571927b0-3a43-4d85-ba0f-f59833bd12d0", "Family", "FAMILY" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialistPatients_PatientId",
                table: "SpecialistPatients",
                column: "PatientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialistPatients_Patients_PatientId",
                table: "SpecialistPatients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
