using Microsoft.EntityFrameworkCore.Migrations;

namespace Alameen_CustomerAndDonglesMangment.Data.Migrations
{
    public partial class EditTaskModelUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Competents_CompetentId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Competents");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CompetentId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CompetentId",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "CompetentId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Competents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CompetentId",
                table: "Tasks",
                column: "CompetentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Competents_CompetentId",
                table: "Tasks",
                column: "CompetentId",
                principalTable: "Competents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
