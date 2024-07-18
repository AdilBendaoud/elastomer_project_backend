using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WESM_articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilleDeProduit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Destination = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_articles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WESM_fournisseurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_fournisseurs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WESM_passwordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Token = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expiration = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_passwordResetTokens", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WESM_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Departement = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NeedsPasswordChange = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPurchaser = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRequester = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsValidator = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WESM_demandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DemandeurId = table.Column<int>(type: "int", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValidateurCFOId = table.Column<int>(type: "int", nullable: true),
                    CommentCFO = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsValidateurCFOValidated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsValidateurCFORejected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ValidatedOrRejectedByCFOAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValidateurCOOId = table.Column<int>(type: "int", nullable: true),
                    CommentCOO = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsValidateurCOOValidated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsValidateurCOORejected = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ValidatedOrRejectedByCOOAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AcheteurId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_demandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WESM_demandes_WESM_users_AcheteurId",
                        column: x => x.AcheteurId,
                        principalTable: "WESM_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WESM_demandes_WESM_users_DemandeurId",
                        column: x => x.DemandeurId,
                        principalTable: "WESM_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WESM_demandes_WESM_users_ValidateurCFOId",
                        column: x => x.ValidateurCFOId,
                        principalTable: "WESM_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WESM_demandes_WESM_users_ValidateurCOOId",
                        column: x => x.ValidateurCOOId,
                        principalTable: "WESM_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "WESM_demandeArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Qtt = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BonCommande = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FamilleDeProduit = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Destination = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_demandeArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WESM_demandeArticles_WESM_articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "WESM_articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WESM_demandeArticles_WESM_demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "WESM_demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WESM_demandeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    UserCode = table.Column<int>(type: "int", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Details = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_demandeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WESM_demandeHistories_WESM_demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "WESM_demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateTable(
                name: "DevisItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DemandeArticleId = table.Column<int>(type: "int", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Devise = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Delay = table.Column<DateOnly>(type: "date", nullable: false),
                    DevisId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevisItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevisItems_WESM_demandeArticles_DemandeArticleId",
                        column: x => x.DemandeArticleId,
                        principalTable: "WESM_demandeArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevisItems_WESM_devis_DevisId",
                        column: x => x.DevisId,
                        principalTable: "WESM_devis",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DevisItems_WESM_fournisseurs_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "WESM_fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_DemandeArticleId",
                table: "DevisItems",
                column: "DemandeArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_DevisId",
                table: "DevisItems",
                column: "DevisId");

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_FournisseurId",
                table: "DevisItems",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequests_DemandeId",
                table: "SupplierRequests",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierRequests_SupplierId",
                table: "SupplierRequests",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandeArticles_ArticleId",
                table: "WESM_demandeArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandeArticles_DemandeId",
                table: "WESM_demandeArticles",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandeHistories_DemandeId",
                table: "WESM_demandeHistories",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandes_AcheteurId",
                table: "WESM_demandes",
                column: "AcheteurId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandes_DemandeurId",
                table: "WESM_demandes",
                column: "DemandeurId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandes_ValidateurCFOId",
                table: "WESM_demandes",
                column: "ValidateurCFOId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_demandes_ValidateurCOOId",
                table: "WESM_demandes",
                column: "ValidateurCOOId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_devis_DemandeId",
                table: "WESM_devis",
                column: "DemandeId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_devis_FournisseurId",
                table: "WESM_devis",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_WESM_passwordResetTokens_Token",
                table: "WESM_passwordResetTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WESM_users_Code",
                table: "WESM_users",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevisItems");

            migrationBuilder.DropTable(
                name: "SupplierRequests");

            migrationBuilder.DropTable(
                name: "WESM_demandeHistories");

            migrationBuilder.DropTable(
                name: "WESM_passwordResetTokens");

            migrationBuilder.DropTable(
                name: "WESM_demandeArticles");

            migrationBuilder.DropTable(
                name: "WESM_devis");

            migrationBuilder.DropTable(
                name: "WESM_articles");

            migrationBuilder.DropTable(
                name: "WESM_demandes");

            migrationBuilder.DropTable(
                name: "WESM_fournisseurs");

            migrationBuilder.DropTable(
                name: "WESM_users");
        }
    }
}
