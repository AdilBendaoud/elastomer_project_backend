using System;
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
            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Departement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    InitialBudget = table.Column<int>(type: "int", nullable: true),
                    SalesBudget = table.Column<int>(type: "int", nullable: true),
                    SalesForecast = table.Column<int>(type: "int", nullable: true),
                    Adjustment = table.Column<int>(type: "int", nullable: true),
                    BudgetIP = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceInEur = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WESM_articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilleDeProduit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WESM_fournisseurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_fournisseurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WESM_passwordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_passwordResetTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WESM_users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Departement = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    NeedsPasswordChange = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsPurchaser = table.Column<bool>(type: "bit", nullable: false),
                    IsRequester = table.Column<bool>(type: "bit", nullable: false),
                    IsValidator = table.Column<bool>(type: "bit", nullable: false),
                    ReOpenRequestAfterValidation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WESM_demandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DemandeurId = table.Column<int>(type: "int", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidateurCFOId = table.Column<int>(type: "int", nullable: true),
                    CommentCFO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsValidateurCFOValidated = table.Column<bool>(type: "bit", nullable: false),
                    IsValidateurCFORejected = table.Column<bool>(type: "bit", nullable: false),
                    ValidatedOrRejectedByCFOAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModification = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidateurCOOId = table.Column<int>(type: "int", nullable: true),
                    CommentCOO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsValidateurCOOValidated = table.Column<bool>(type: "bit", nullable: false),
                    IsValidateurCOORejected = table.Column<bool>(type: "bit", nullable: false),
                    ValidatedOrRejectedByCOOAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "SupplierRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    isSelectedForValidation = table.Column<bool>(type: "bit", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "WESM_demandeArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Qtt = table.Column<int>(type: "int", nullable: false),
                    BonCommande = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilleDeProduit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WESM_demandeArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WESM_demandeArticles_WESM_demandes_DemandeId",
                        column: x => x.DemandeId,
                        principalTable: "WESM_demandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WESM_demandeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandeId = table.Column<int>(type: "int", nullable: false),
                    UserCode = table.Column<int>(type: "int", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "DevisItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandeArticleId = table.Column<int>(type: "int", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: true),
                    Devise = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Delay = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                        name: "FK_DevisItems_WESM_fournisseurs_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "WESM_fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DevisItems_DemandeArticleId",
                table: "DevisItems",
                column: "DemandeArticleId");

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
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "DevisItems");

            migrationBuilder.DropTable(
                name: "SupplierRequests");

            migrationBuilder.DropTable(
                name: "WESM_articles");

            migrationBuilder.DropTable(
                name: "WESM_demandeHistories");

            migrationBuilder.DropTable(
                name: "WESM_passwordResetTokens");

            migrationBuilder.DropTable(
                name: "WESM_demandeArticles");

            migrationBuilder.DropTable(
                name: "WESM_fournisseurs");

            migrationBuilder.DropTable(
                name: "WESM_demandes");

            migrationBuilder.DropTable(
                name: "WESM_users");
        }
    }
}
