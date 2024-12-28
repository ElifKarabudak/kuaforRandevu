using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirKadınBirErkekTasarımMerkezi.Data.Migrations
{
    public partial class sinifg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kisims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KisimAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kapasitesi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kisims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ustalars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UstalikAlaniAciklama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ustalars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "islemlers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sure = table.Column<int>(type: "int", nullable: false),
                    Ucret = table.Column<int>(type: "int", nullable: false),
                    KisimID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_islemlers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_islemlers_kisims_KisimID",
                        column: x => x.KisimID,
                        principalTable: "kisims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "randevus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iletisimNumaraniz = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    kullaniciID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IslemlerID = table.Column<int>(type: "int", nullable: false),
                    UstalarID = table.Column<int>(type: "int", nullable: false),
                    Zaman = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_randevus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_randevus_islemlers_IslemlerID",
                        column: x => x.IslemlerID,
                        principalTable: "islemlers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_randevus_ustalars_UstalarID",
                        column: x => x.UstalarID,
                        principalTable: "ustalars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ustalarinYapabildigiIslemlers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UstalarID = table.Column<int>(type: "int", nullable: false),
                    IslemlerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ustalarinYapabildigiIslemlers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ustalarinYapabildigiIslemlers_islemlers_IslemlerID",
                        column: x => x.IslemlerID,
                        principalTable: "islemlers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ustalarinYapabildigiIslemlers_ustalars_UstalarID",
                        column: x => x.UstalarID,
                        principalTable: "ustalars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_islemlers_KisimID",
                table: "islemlers",
                column: "KisimID");

            migrationBuilder.CreateIndex(
                name: "IX_randevus_IslemlerID",
                table: "randevus",
                column: "IslemlerID");

            migrationBuilder.CreateIndex(
                name: "IX_randevus_UstalarID",
                table: "randevus",
                column: "UstalarID");

            migrationBuilder.CreateIndex(
                name: "IX_ustalarinYapabildigiIslemlers_IslemlerID",
                table: "ustalarinYapabildigiIslemlers",
                column: "IslemlerID");

            migrationBuilder.CreateIndex(
                name: "IX_ustalarinYapabildigiIslemlers_UstalarID",
                table: "ustalarinYapabildigiIslemlers",
                column: "UstalarID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "randevus");

            migrationBuilder.DropTable(
                name: "ustalarinYapabildigiIslemlers");

            migrationBuilder.DropTable(
                name: "islemlers");

            migrationBuilder.DropTable(
                name: "ustalars");

            migrationBuilder.DropTable(
                name: "kisims");
        }
    }
}
