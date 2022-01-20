using Microsoft.EntityFrameworkCore.Migrations;

namespace Alameen_CustomerAndDonglesMangment.Data.Migrations
{
    public partial class AddIsPaidToTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Tasks",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Tasks");
        }
    }
}
