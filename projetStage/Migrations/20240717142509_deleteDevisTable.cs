using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class deleteDevisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevisItems_WESM_devis_DevisId",
                table: "DevisItems");

            migrationBuilder.DropTable(
                name: "WESM_devis");

            migrationBuilder.RenameColumn(
                name: "DevisId",
                table: "DevisItems",
                newName: "DemandeId");

            migrationBuilder.RenameIndex(
                name: "IX_DevisItems_DevisId",
                table: "DevisItems",
                newName: "IX_DevisItems_DemandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevisItems_WESM_demandes_DemandeId",
                table: "DevisItems",
                column: "DemandeId",
                principalTable: "WESM_demandes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevisItems_WESM_demandes_DemandeId",
                table: "DevisItems");

            migrationBuilder.RenameColumn(
                name: "DemandeId",
                table: "DevisItems",
                newName: "DevisId");

            migrationBuilder.RenameIndex(
                name: "IX_DevisItems_DemandeId",
                table: "DevisItems",
                newName: "IX_DevisItems_DevisId");

            migrationBuilder.CreateTable(
                name: "WESM_devis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_devis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WESM_devis_WESM_demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "WESM_demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WESM_devis_WESM_fournisseurs_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "WESM_fournisseurs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_devis_DemandeId",
                table: "WESM_devis",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_devis_FournisseurId",
                table: "WESM_devis",
                column: "FournisseurId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevisItems_WESM_devis_DevisId",
                table: "DevisItems",
                column: "DevisId",
                principalTable: "WESM_devis",
                principalColumn: "Id");
        }
    }
}
