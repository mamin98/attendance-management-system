using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_AttendancePolicy_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AttendancePolicy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxLateMinutesPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxPermissionsPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxRemoteDaysPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxEarlyLeavesPerMonth = table.Column<int>(type: "int", nullable: false),
                    RequiresManagerApproval = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancePolicy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_PolicyId",
                table: "Departments",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AttendancePolicy_PolicyId",
                table: "Departments",
                column: "PolicyId",
                principalTable: "AttendancePolicy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AttendancePolicy_PolicyId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "AttendancePolicy");

            migrationBuilder.DropIndex(
                name: "IX_Departments_PolicyId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Departments");
        }
    }
}
