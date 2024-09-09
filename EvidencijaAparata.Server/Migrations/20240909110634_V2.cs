using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvidencijaAparata.Server.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "GMBaseActs",
                columns: new[] { "Id", "DatumAkt", "DatumDeakt", "GMBaseId", "GMLocationActId", "ResenjeAkt", "ResenjeDeakt" },
                values: new object[,]
                {
                    { 1, new DateOnly(2024, 1, 9), new DateOnly(2024, 3, 9), 5, 1, "ResenjeAkt1", "ResenjeDeakt1" },
                    { 2, new DateOnly(2024, 4, 9), new DateOnly(2024, 6, 9), 4, 3, "ResenjeAkt2", "ResenjeDeakt2" },
                    { 3, new DateOnly(2024, 7, 9), null, 3, 3, "ResenjeAkt3", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GMBaseActs_GMBaseId",
                table: "GMBaseActs",
                column: "GMBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_GMBaseActs_GMLocationActId",
                table: "GMBaseActs",
                column: "GMLocationActId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GMBaseActs");

            migrationBuilder.DropTable(
                name: "GMBases");
        }
    }
}
