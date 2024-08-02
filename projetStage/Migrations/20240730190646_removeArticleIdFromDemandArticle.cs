using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class removeArticleIdFromDemandArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WESM_demandeArticles_WESM_articles_ArticleId",
                table: "WESM_demandeArticles");

            migrationBuilder.DropIndex(
                name: "IX_WESM_demandeArticles_ArticleId",
                table: "WESM_demandeArticles");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "WESM_demandeArticles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "WESM_demandeArticles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandeArticles_ArticleId",
                table: "WESM_demandeArticles",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_WESM_demandeArticles_WESM_articles_ArticleId",
                table: "WESM_demandeArticles",
                column: "ArticleId",
                principalTable: "WESM_articles",
                principalColumn: "Id");
        }
    }
}
