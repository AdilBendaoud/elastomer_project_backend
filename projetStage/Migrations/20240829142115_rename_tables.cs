using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class rename_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevisItems_WESM_demandeArticles_DemandeArticleId",
                table: "DevisItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DevisItems_WESM_fournisseurs_FournisseurId",
                table: "DevisItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierRequests_WESM_demandes_DemandeId",
                table: "SupplierRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierRequests_WESM_fournisseurs_SupplierId",
                table: "SupplierRequests");

            migrationBuilder.DropTable(
                name: "WESM_articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplierRequests",
                table: "SupplierRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DevisItems",
                table: "DevisItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "SupplierRequests",
                newName: "WESM_supplierRequests");

            migrationBuilder.RenameTable(
                name: "DevisItems",
                newName: "WESM_devisItems");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "WESM_currencies");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "WESM_Budgets");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierRequests_SupplierId",
                table: "WESM_supplierRequests",
                newName: "IX_WESM_supplierRequests_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_SupplierRequests_DemandeId",
                table: "WESM_supplierRequests",
                newName: "IX_WESM_supplierRequests_DemandeId");

            migrationBuilder.RenameIndex(
                name: "IX_DevisItems_FournisseurId",
                table: "WESM_devisItems",
                newName: "IX_WESM_devisItems_FournisseurId");

            migrationBuilder.RenameIndex(
                name: "IX_DevisItems_DemandeArticleId",
                table: "WESM_devisItems",
                newName: "IX_WESM_devisItems_DemandeArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WESM_supplierRequests",
                table: "WESM_supplierRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WESM_devisItems",
                table: "WESM_devisItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WESM_currencies",
                table: "WESM_currencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WESM_Budgets",
                table: "WESM_Budgets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WESM_devisItems_WESM_demandeArticles_DemandeArticleId",
                table: "WESM_devisItems",
                column: "DemandeArticleId",
                principalTable: "WESM_demandeArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WESM_devisItems_WESM_fournisseurs_FournisseurId",
                table: "WESM_devisItems",
                column: "FournisseurId",
                principalTable: "WESM_fournisseurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WESM_supplierRequests_WESM_demandes_DemandeId",
                table: "WESM_supplierRequests",
                column: "DemandeId",
                principalTable: "WESM_demandes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WESM_supplierRequests_WESM_fournisseurs_SupplierId",
                table: "WESM_supplierRequests",
                column: "SupplierId",
                principalTable: "WESM_fournisseurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WESM_devisItems_WESM_demandeArticles_DemandeArticleId",
                table: "WESM_devisItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WESM_devisItems_WESM_fournisseurs_FournisseurId",
                table: "WESM_devisItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WESM_supplierRequests_WESM_demandes_DemandeId",
                table: "WESM_supplierRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_WESM_supplierRequests_WESM_fournisseurs_SupplierId",
                table: "WESM_supplierRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WESM_supplierRequests",
                table: "WESM_supplierRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WESM_devisItems",
                table: "WESM_devisItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WESM_currencies",
                table: "WESM_currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WESM_Budgets",
                table: "WESM_Budgets");

            migrationBuilder.RenameTable(
                name: "WESM_supplierRequests",
                newName: "SupplierRequests");

            migrationBuilder.RenameTable(
                name: "WESM_devisItems",
                newName: "DevisItems");

            migrationBuilder.RenameTable(
                name: "WESM_currencies",
                newName: "Currencies");

            migrationBuilder.RenameTable(
                name: "WESM_Budgets",
                newName: "Budgets");

            migrationBuilder.RenameIndex(
                name: "IX_WESM_supplierRequests_SupplierId",
                table: "SupplierRequests",
                newName: "IX_SupplierRequests_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_WESM_supplierRequests_DemandeId",
                table: "SupplierRequests",
                newName: "IX_SupplierRequests_DemandeId");

            migrationBuilder.RenameIndex(
                name: "IX_WESM_devisItems_FournisseurId",
                table: "DevisItems",
                newName: "IX_DevisItems_FournisseurId");

            migrationBuilder.RenameIndex(
                name: "IX_WESM_devisItems_DemandeArticleId",
                table: "DevisItems",
                newName: "IX_DevisItems_DemandeArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplierRequests",
                table: "SupplierRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DevisItems",
                table: "DevisItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WESM_articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilleDeProduit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_articles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DevisItems_WESM_demandeArticles_DemandeArticleId",
                table: "DevisItems",
                column: "DemandeArticleId",
                principalTable: "WESM_demandeArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DevisItems_WESM_fournisseurs_FournisseurId",
                table: "DevisItems",
                column: "FournisseurId",
                principalTable: "WESM_fournisseurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierRequests_WESM_demandes_DemandeId",
                table: "SupplierRequests",
                column: "DemandeId",
                principalTable: "WESM_demandes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierRequests_WESM_fournisseurs_SupplierId",
                table: "SupplierRequests",
                column: "SupplierId",
                principalTable: "WESM_fournisseurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
