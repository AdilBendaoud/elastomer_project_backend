using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class deleteDemandeIdFromDevisItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevisItems_WESM_demandes_DemandeId",
                table: "DevisItems");

            migrationBuilder.DropIndex(
                name: "IX_DevisItems_DemandeId",
                table: "DevisItems");

            migrationBuilder.DropColumn(
                name: "DemandeId",
                table: "DevisItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DemandeId",
                table: "DevisItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_DemandeId",
                table: "DevisItems",
                column: "DemandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevisItems_WESM_demandes_DemandeId",
                table: "DevisItems",
                column: "DemandeId",
                principalTable: "WESM_demandes",
                principalColumn: "Id");
        }
    }
}
