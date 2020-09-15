using Microsoft.EntityFrameworkCore.Migrations;

namespace MagTech.DataAccess.Migrations
{
    public partial class KtoUtworzylDoNaglowkaKoszyk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KtoUtworzyl",
                table: "NaglowekKoszykZadania",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KtoUtworzyl",
                table: "NaglowekKoszykZadania");
        }
    }
}
