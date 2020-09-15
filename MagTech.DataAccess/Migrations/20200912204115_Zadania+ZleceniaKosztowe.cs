using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MagTech.DataAccess.Migrations
{
    public partial class ZadaniaZleceniaKosztowe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaglowekKoszykZadania",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Priorytet = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaglowekKoszykZadania", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZleceniaKosztowe",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Linia = table.Column<string>(nullable: false),
                    Maszyna = table.Column<string>(nullable: true),
                    MPK = table.Column<string>(nullable: true),
                    Zlecenie = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZleceniaKosztowe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KoszykZadanie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NaglowekId = table.Column<int>(nullable: false),
                    Proces = table.Column<string>(nullable: false),
                    ArtykulId = table.Column<int>(nullable: false),
                    Ilosc = table.Column<int>(nullable: false),
                    Koszty = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoszykZadanie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoszykZadanie_ZbiorArtykolow_ArtykulId",
                        column: x => x.ArtykulId,
                        principalTable: "ZbiorArtykolow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KoszykZadanie_NaglowekKoszykZadania_NaglowekId",
                        column: x => x.NaglowekId,
                        principalTable: "NaglowekKoszykZadania",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KoszykZadanie_ArtykulId",
                table: "KoszykZadanie",
                column: "ArtykulId");

            migrationBuilder.CreateIndex(
                name: "IX_KoszykZadanie_NaglowekId",
                table: "KoszykZadanie",
                column: "NaglowekId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KoszykZadanie");

            migrationBuilder.DropTable(
                name: "ZleceniaKosztowe");

            migrationBuilder.DropTable(
                name: "NaglowekKoszykZadania");
        }
    }
}
