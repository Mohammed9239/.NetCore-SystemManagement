using Microsoft.EntityFrameworkCore.Migrations;

namespace Alameen_CustomerAndDonglesMangment.Data.Migrations
{
    public partial class AddMaintenanceTypeAndCostToTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Tasks",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MaintenanceTypeId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MaintenanceType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_MaintenanceTypeId",
                table: "Tasks",
                column: "MaintenanceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_MaintenanceType_MaintenanceTypeId",
                table: "Tasks",
                column: "MaintenanceTypeId",
                principalTable: "MaintenanceType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_MaintenanceType_MaintenanceTypeId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "MaintenanceType");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_MaintenanceTypeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "MaintenanceTypeId",
                table: "Tasks");
        }
    }
}
