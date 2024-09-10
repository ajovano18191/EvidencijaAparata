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
                name: "GMBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    serial_num = table.Column<string>(type: "text", nullable: false),
                    old_sticker_no = table.Column<string>(type: "text", nullable: false),
                    work_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GMBases", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "GMBaseActs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DatumAkt = table.Column<DateOnly>(type: "date", nullable: false),
                    ResenjeAkt = table.Column<string>(type: "text", nullable: false),
                    DatumDeakt = table.Column<DateOnly>(type: "date", nullable: true),
                    ResenjeDeakt = table.Column<string>(type: "text", nullable: true),
                    GMBaseId = table.Column<int>(type: "integer", nullable: false),
                    GMLocationActId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GMBaseActs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GMBaseActs_GMBases_GMBaseId",
                        column: x => x.GMBaseId,
                        principalTable: "GMBases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GMBaseActs_GMLocationActs_GMLocationActId",
                        column: x => x.GMLocationActId,
                        principalTable: "GMLocationActs",
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
                table: "GMBases",
                columns: new[] { "Id", "Name", "old_sticker_no", "serial_num", "work_type" },
                values: new object[,]
                {
                    { 1, "GM 1", "OSN1", "SN1", "APOLLO" },
                    { 2, "GM 2", "OSN2", "SN2", "SAS" },
                    { 3, "GM 3", "OSN3", "SN3", "ROULETE" },
                    { 4, "GM 4", "OSN4", "SN4", "APOLLO" },
                    { 5, "GM 5", "OSN5", "SN5", "SAS" }
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
                    { 1, new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), 1, "Napomena 1", "ResenjeAkt1", "ResenjeDeakt1" },
                    { 2, new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 1), 2, "Napomena 2", "ResenjeAkt2", "ResenjeDeakt2" },
                    { 3, new DateOnly(2024, 9, 4), null, 2, "Napomena 3", "ResenjeAkt3", null }
                });

            migrationBuilder.InsertData(
                table: "GMBaseActs",
                columns: new[] { "Id", "DatumAkt", "DatumDeakt", "GMBaseId", "GMLocationActId", "ResenjeAkt", "ResenjeDeakt" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), 5, 1, "ResenjeAkt1", "ResenjeDeakt1" },
                    { 2, new DateOnly(2024, 9, 4), new DateOnly(2024, 9, 6), 4, 3, "ResenjeAkt2", "ResenjeDeakt2" },
                    { 3, new DateOnly(2024, 9, 7), null, 3, 3, "ResenjeAkt3", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GMBaseActs_GMBaseId",
                table: "GMBaseActs",
                column: "GMBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_GMBaseActs_GMLocationActId",
                table: "GMBaseActs",
                column: "GMLocationActId");

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
                name: "GMBaseActs");

            migrationBuilder.DropTable(
                name: "GMBases");

            migrationBuilder.DropTable(
                name: "GMLocationActs");

            migrationBuilder.DropTable(
                name: "GMLocations");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
