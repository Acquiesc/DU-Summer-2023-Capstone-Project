using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DU_Summer_2023_Capstone.Data.Migrations
{
    public partial class UpdateMig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Orders");
        }
    }
}
