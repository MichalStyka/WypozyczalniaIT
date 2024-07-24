using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wypozyczalnia.Data.Migrations
{
    /// <inheritdoc />
    public partial class wypozyczenieCenaupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CenaCalkowita",
                table: "Wypozycenia",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CenaCalkowita",
                table: "Wypozycenia",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
