using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EnvanterTakip.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    AppUserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KullaniciAdi = table.Column<string>(type: "text", nullable: true),
                    Sifre = table.Column<string>(type: "text", nullable: true),
                    Rol = table.Column<string>(type: "text", nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.AppUserID);
                });

            migrationBuilder.CreateTable(
                name: "Cihazlar",
                columns: table => new
                {
                    CihazID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CihazTuru = table.Column<string>(type: "text", nullable: true),
                    Marka = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: true),
                    SeriNo = table.Column<string>(type: "text", nullable: true),
                    AlimTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GarantiBitisTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true),
                    Durum = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cihazlar", x => x.CihazID);
                });

            migrationBuilder.CreateTable(
                name: "Personeller",
                columns: table => new
                {
                    PersonelID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "text", nullable: true),
                    Soyad = table.Column<string>(type: "text", nullable: true),
                    BirimAdi = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Telefon = table.Column<string>(type: "text", nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personeller", x => x.PersonelID);
                });

            migrationBuilder.CreateTable(
                name: "Zimmetler",
                columns: table => new
                {
                    ZimmetID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CihazID = table.Column<int>(type: "integer", nullable: false),
                    PersonelID = table.Column<int>(type: "integer", nullable: false),
                    ZimmetTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IadeTarihi = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Aciklama = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zimmetler", x => x.ZimmetID);
                    table.ForeignKey(
                        name: "FK_Zimmetler_Cihazlar_CihazID",
                        column: x => x.CihazID,
                        principalTable: "Cihazlar",
                        principalColumn: "CihazID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zimmetler_Personeller_PersonelID",
                        column: x => x.PersonelID,
                        principalTable: "Personeller",
                        principalColumn: "PersonelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zimmetler_CihazID",
                table: "Zimmetler",
                column: "CihazID");

            migrationBuilder.CreateIndex(
                name: "IX_Zimmetler_PersonelID",
                table: "Zimmetler",
                column: "PersonelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Zimmetler");

            migrationBuilder.DropTable(
                name: "Cihazlar");

            migrationBuilder.DropTable(
                name: "Personeller");
        }
    }
}
