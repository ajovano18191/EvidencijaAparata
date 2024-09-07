using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvidencijaAparata.Server.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Naziv = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GMLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rul_base_id = table.Column<int>(type: "integer", nullable: false),
                    Naziv = table.Column<string>(type: "text", nullable: false),
                    Adresa = table.Column<string>(type: "text", nullable: false),
                    MestoId = table.Column<int>(type: "integer", nullable: false),
                    IP = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GMLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GMLocations_Cities_MestoId",
                        column: x => x.MestoId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GMLocationActs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatumAkt = table.Column<DateOnly>(type: "date", nullable: false),
                    ResenjeAkt = table.Column<string>(type: "text", nullable: false),
                    DatumDeakt = table.Column<DateOnly>(type: "date", nullable: true),
                    ResenjeDeakt = table.Column<string>(type: "text", nullable: true),
                    Napomena = table.Column<string>(type: "text", nullable: false),
                    GMLocationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GMLocationActs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GMLocationActs_GMLocations_GMLocationId",
                        column: x => x.GMLocationId,
                        principalTable: "GMLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Naziv" },
                values: new object[,]
                {
                    { 1, "City 1" },
                    { 2, "City 2" },
                    { 3, "City 3" },
                    { 4, "City 4" },
                    { 5, "City 5" }
                });

            migrationBuilder.InsertData(
                table: "GMLocations",
                columns: new[] { "Id", "Adresa", "IP", "MestoId", "Naziv", "rul_base_id" },
                values: new object[,]
                {
                    { 1, "Adresa 1", "192.168.0.1", 1, "Lokacija 1", 1 },
                    { 2, "Adresa 2", "192.168.0.2", 2, "Lokacija 2", 2 },
                    { 3, "Adresa 3", "192.168.0.3", 3, "Lokacija 3", 3 },
                    { 4, "Adresa 4", "192.168.0.4", 4, "Lokacija 4", 4 },
                    { 5, "Adresa 5", "192.168.0.5", 5, "Lokacija 5", 5 }
                });

            migrationBuilder.InsertData(
                table: "GMLocationActs",
                columns: new[] { "Id", "DatumAkt", "DatumDeakt", "GMLocationId", "Napomena", "ResenjeAkt", "ResenjeDeakt" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 1, 9), new DateOnly(2024, 3, 9), 1, "Napomena 1", "ResenjeAkt1", "ResenjeDeakt1" },
                    { 2, new DateOnly(2024, 1, 9), new DateOnly(2024, 3, 9), 2, "Napomena 2", "ResenjeAkt2", "ResenjeDeakt2" },
                    { 3, new DateOnly(2024, 4, 9), null, 2, "Napomena 3", "ResenjeAkt3", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GMLocationActs_GMLocationId",
                table: "GMLocationActs",
                column: "GMLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GMLocations_MestoId",
                table: "GMLocations",
                column: "MestoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GMLocationActs");

            migrationBuilder.DropTable(
                name: "GMLocations");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
