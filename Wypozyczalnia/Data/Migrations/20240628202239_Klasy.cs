using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wypozyczalnia.Data.Migrations
{
    /// <inheritdoc />
    public partial class Klasy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorie",
                columns: table => new
                {
                    KategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriaNazwa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie", x => x.KategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Sprzety",
                columns: table => new
                {
                    SprzetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SprzetNazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KategoriaId = table.Column<int>(type: "int", nullable: true),
                    CenaZaDzien = table.Column<double>(type: "float", nullable: false),
                    CzyDostepne = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprzety", x => x.SprzetId);
                    table.ForeignKey(
                        name: "FK_Sprzety_Kategorie_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "Kategorie",
                        principalColumn: "KategoriaId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Wypozycenia",
                columns: table => new
                {
                    WypozyczenieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SprzetId = table.Column<int>(type: "int", nullable: false),
                    DataWypozyczenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataZwrotu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CenaCalkowita = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wypozycenia", x => x.WypozyczenieId);
                    table.ForeignKey(
                        name: "FK_Wypozycenia_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Wypozycenia_Sprzety_SprzetId",
                        column: x => x.SprzetId,
                        principalTable: "Sprzety",
                        principalColumn: "SprzetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sprzety_KategoriaId",
                table: "Sprzety",
                column: "KategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozycenia_SprzetId",
                table: "Wypozycenia",
                column: "SprzetId");

            migrationBuilder.CreateIndex(
                name: "IX_Wypozycenia_UserId",
                table: "Wypozycenia",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wypozycenia");

            migrationBuilder.DropTable(
                name: "Sprzety");

            migrationBuilder.DropTable(
                name: "Kategorie");
        }
    }
}
