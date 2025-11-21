using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnvanterTakip.Migrations
{
    /// <inheritdoc />
    public partial class AddEkleyenKullaniciToKonum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EkleyenKullanici",
                table: "Konumlar",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EkleyenKullanici",
                table: "Konumlar");
        }
    }
}
