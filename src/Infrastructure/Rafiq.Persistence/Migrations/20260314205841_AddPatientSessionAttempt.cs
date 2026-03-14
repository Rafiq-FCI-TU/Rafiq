using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rafiq.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientSessionAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientSessionAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "PendingReview"),
                    DoctorFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoreAwarded = table.Column<int>(type: "int", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSessionAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientSessionAttempts_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientSessionAttempts_Session_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientSessionAttempts_PatientId_SessionId",
                table: "PatientSessionAttempts",
                columns: new[] { "PatientId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_PatientSessionAttempts_SessionId",
                table: "PatientSessionAttempts",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientSessionAttempts");
        }
    }
}
