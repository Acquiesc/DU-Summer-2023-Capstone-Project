using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DU_Summer_2023_Capstone.Data.Migrations
{
    public partial class UpdateMig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AspNetUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AspNetUserId",
                table: "Orders",
                column: "AspNetUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_AspNetUserId",
                table: "Orders",
                column: "AspNetUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_AspNetUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_AspNetUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AspNetUserId",
                table: "Orders");
        }
    }
}
