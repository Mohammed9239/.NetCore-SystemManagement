using Microsoft.EntityFrameworkCore.Migrations;

namespace Alameen_CustomerAndDonglesMangment.Data.Migrations
{
    public partial class AddMaintenanceTypeAndCostToTasksToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_MaintenanceType_MaintenanceTypeId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceType",
                table: "MaintenanceType");

            migrationBuilder.RenameTable(
                name: "MaintenanceType",
                newName: "MaintenanceTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceTypes",
                table: "MaintenanceTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_MaintenanceTypes_MaintenanceTypeId",
                table: "Tasks",
                column: "MaintenanceTypeId",
                principalTable: "MaintenanceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_MaintenanceTypes_MaintenanceTypeId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaintenanceTypes",
                table: "MaintenanceTypes");

            migrationBuilder.RenameTable(
                name: "MaintenanceTypes",
                newName: "MaintenanceType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaintenanceType",
                table: "MaintenanceType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_MaintenanceType_MaintenanceTypeId",
                table: "Tasks",
                column: "MaintenanceTypeId",
                principalTable: "MaintenanceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
