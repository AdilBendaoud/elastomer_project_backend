using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class addFamilleDeproduitToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "WESM_demandeArticles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FamilleDeProduit",
                table: "WESM_demandeArticles",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "WESM_articles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FamilleDeProduit",
                table: "WESM_articles",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "WESM_demandeArticles");

            migrationBuilder.DropColumn(
                name: "FamilleDeProduit",
                table: "WESM_demandeArticles");

            migrationBuilder.DropColumn(
                name: "Destination",
                table: "WESM_articles");

            migrationBuilder.DropColumn(
                name: "FamilleDeProduit",
                table: "WESM_articles");
        }
    }
}
