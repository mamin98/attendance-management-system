using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendanceSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAttendancePolicyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShift_Employees_EmployeeId",
                table: "EmployeeShift");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShift_Shift_ShiftId",
                table: "EmployeeShift");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDay_Shift_ShiftId",
                table: "ShiftDay");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDayDetail_ShiftDay_ShiftDayId",
                table: "ShiftDayDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDayDetail_Shift_ShiftId",
                table: "ShiftDayDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftDayDetail",
                table: "ShiftDayDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftDay",
                table: "ShiftDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shift",
                table: "Shift");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShift",
                table: "EmployeeShift");

            migrationBuilder.RenameTable(
                name: "ShiftDayDetail",
                newName: "ShiftDayDetails");

            migrationBuilder.RenameTable(
                name: "ShiftDay",
                newName: "ShiftDays");

            migrationBuilder.RenameTable(
                name: "Shift",
                newName: "Shifts");

            migrationBuilder.RenameTable(
                name: "EmployeeShift",
                newName: "EmployeeShifts");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDayDetail_ShiftId",
                table: "ShiftDayDetails",
                newName: "IX_ShiftDayDetails_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDayDetail_ShiftDayId",
                table: "ShiftDayDetails",
                newName: "IX_ShiftDayDetails_ShiftDayId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDay_ShiftId",
                table: "ShiftDays",
                newName: "IX_ShiftDays_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeShift_ShiftId",
                table: "EmployeeShifts",
                newName: "IX_EmployeeShifts_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeShift_EmployeeId",
                table: "EmployeeShifts",
                newName: "IX_EmployeeShifts_EmployeeId");

            migrationBuilder.AddColumn<Guid>(
                name: "PolicyId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftDayDetails",
                table: "ShiftDayDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftDays",
                table: "ShiftDays",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AttendancePolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameEnglish = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NameArabic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MaxLateMinutesPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxPermissionsPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxRemoteDaysPerMonth = table.Column<int>(type: "int", nullable: false),
                    MaxEarlyLeavesPerMonth = table.Column<int>(type: "int", nullable: false),
                    RequiresManagerApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancePolicies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_PolicyId",
                table: "Departments",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_AttendancePolicies_PolicyId",
                table: "Departments",
                column: "PolicyId",
                principalTable: "AttendancePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShifts_Employees_EmployeeId",
                table: "EmployeeShifts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShifts_Shifts_ShiftId",
                table: "EmployeeShifts",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDayDetails_ShiftDays_ShiftDayId",
                table: "ShiftDayDetails",
                column: "ShiftDayId",
                principalTable: "ShiftDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDayDetails_Shifts_ShiftId",
                table: "ShiftDayDetails",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDays_Shifts_ShiftId",
                table: "ShiftDays",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_AttendancePolicies_PolicyId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShifts_Employees_EmployeeId",
                table: "EmployeeShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeShifts_Shifts_ShiftId",
                table: "EmployeeShifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDayDetails_ShiftDays_ShiftDayId",
                table: "ShiftDayDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDayDetails_Shifts_ShiftId",
                table: "ShiftDayDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftDays_Shifts_ShiftId",
                table: "ShiftDays");

            migrationBuilder.DropTable(
                name: "AttendancePolicies");

            migrationBuilder.DropIndex(
                name: "IX_Departments_PolicyId",
                table: "Departments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shifts",
                table: "Shifts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftDays",
                table: "ShiftDays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShiftDayDetails",
                table: "ShiftDayDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeShifts",
                table: "EmployeeShifts");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "Shifts",
                newName: "Shift");

            migrationBuilder.RenameTable(
                name: "ShiftDays",
                newName: "ShiftDay");

            migrationBuilder.RenameTable(
                name: "ShiftDayDetails",
                newName: "ShiftDayDetail");

            migrationBuilder.RenameTable(
                name: "EmployeeShifts",
                newName: "EmployeeShift");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDays_ShiftId",
                table: "ShiftDay",
                newName: "IX_ShiftDay_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDayDetails_ShiftId",
                table: "ShiftDayDetail",
                newName: "IX_ShiftDayDetail_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_ShiftDayDetails_ShiftDayId",
                table: "ShiftDayDetail",
                newName: "IX_ShiftDayDetail_ShiftDayId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeShifts_ShiftId",
                table: "EmployeeShift",
                newName: "IX_EmployeeShift_ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeShifts_EmployeeId",
                table: "EmployeeShift",
                newName: "IX_EmployeeShift_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shift",
                table: "Shift",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftDay",
                table: "ShiftDay",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShiftDayDetail",
                table: "ShiftDayDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeShift",
                table: "EmployeeShift",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShift_Employees_EmployeeId",
                table: "EmployeeShift",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeShift_Shift_ShiftId",
                table: "EmployeeShift",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDay_Shift_ShiftId",
                table: "ShiftDay",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDayDetail_ShiftDay_ShiftDayId",
                table: "ShiftDayDetail",
                column: "ShiftDayId",
                principalTable: "ShiftDay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftDayDetail_Shift_ShiftId",
                table: "ShiftDayDetail",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
