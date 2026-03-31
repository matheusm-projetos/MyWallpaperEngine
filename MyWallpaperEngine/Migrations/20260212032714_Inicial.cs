using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWallpaperEngine.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallpapers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CaminhoCompleto = table.Column<string>(type: "TEXT", nullable: false),
                    NomeExibicao = table.Column<string>(type: "TEXT", nullable: false),
                    CaminhoThumb = table.Column<string>(type: "TEXT", nullable: false),
                    Favorito = table.Column<bool>(type: "INTEGER", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataAdicao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallpapers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallpapers");
        }
    }
}
