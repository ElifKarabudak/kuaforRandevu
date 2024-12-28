using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirKadınBirErkekTasarımMerkezi.Data.Migrations
{
    public partial class onay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Onay",
                table: "randevus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Onay",
                table: "randevus");
        }
    }
}
