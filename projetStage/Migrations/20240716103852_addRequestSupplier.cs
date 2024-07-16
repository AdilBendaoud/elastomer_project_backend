using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class addRequestSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierRequests_WESM_demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "WESM_demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierRequests_WESM_fournisseurs_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "WESM_fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequests_DemandeId",
                table: "SupplierRequests",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequests_SupplierId",
                table: "SupplierRequests",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierRequests");
        }
    }
}
