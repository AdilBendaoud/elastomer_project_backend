using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class demandeFixedAndValidateur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurId1",
                table: "Demandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurId2",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "IsValidateur1Validated",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "IsValidateur2Validated",
                table: "Demandes");

            migrationBuilder.RenameColumn(
                name: "ValidateurId2",
                table: "Demandes",
                newName: "ValidateurCOOId");

            migrationBuilder.RenameColumn(
                name: "ValidateurId1",
                table: "Demandes",
                newName: "ValidateurCFOId");

            migrationBuilder.RenameColumn(
                name: "ValidatedByV2At",
                table: "Demandes",
                newName: "ValidatedByCOOAt");

            migrationBuilder.RenameColumn(
                name: "ValidatedByV1At",
                table: "Demandes",
                newName: "ValidatedByCFOAt");

            migrationBuilder.RenameColumn(
                name: "Num",
                table: "Demandes",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "Comment2",
                table: "Demandes",
                newName: "CommentCOO");

            migrationBuilder.RenameColumn(
                name: "Comment1",
                table: "Demandes",
                newName: "CommentCFO");

            migrationBuilder.RenameIndex(
                name: "IX_Demandes_ValidateurId2",
                table: "Demandes",
                newName: "IX_Demandes_ValidateurCOOId");

            migrationBuilder.RenameIndex(
                name: "IX_Demandes_ValidateurId1",
                table: "Demandes",
                newName: "IX_Demandes_ValidateurCFOId");

            migrationBuilder.RenameIndex(
                name: "IX_Demandes_Num",
                table: "Demandes",
                newName: "IX_Demandes_Code");

            migrationBuilder.AddColumn<int>(
                name: "Qtt",
                table: "Devis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidateurCFOValidated",
                table: "Demandes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidateurCOOValidated",
                table: "Demandes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurCFOId",
                table: "Demandes",
                column: "ValidateurCFOId",
                principalTable: "Validateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurCOOId",
                table: "Demandes",
                column: "ValidateurCOOId",
                principalTable: "Validateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurCFOId",
                table: "Demandes");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurCOOId",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "Qtt",
                table: "Devis");

            migrationBuilder.DropColumn(
                name: "IsValidateurCFOValidated",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "IsValidateurCOOValidated",
                table: "Demandes");

            migrationBuilder.RenameColumn(
                name: "ValidateurCOOId",
                table: "Demandes",
                newName: "ValidateurId2");

            migrationBuilder.RenameColumn(
                name: "ValidateurCFOId",
                table: "Demandes",
                newName: "ValidateurId1");

            migrationBuilder.RenameColumn(
                name: "ValidatedByCOOAt",
                table: "Demandes",
                newName: "ValidatedByV2At");

            migrationBuilder.RenameColumn(
                name: "ValidatedByCFOAt",
                table: "Demandes",
                newName: "ValidatedByV1At");

            migrationBuilder.RenameColumn(
                name: "CommentCOO",
                table: "Demandes",
                newName: "Comment2");

            migrationBuilder.RenameColumn(
                name: "CommentCFO",
                table: "Demandes",
                newName: "Comment1");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Demandes",
                newName: "Num");

            migrationBuilder.RenameIndex(
                name: "IX_Demandes_ValidateurCOOId",
                table: "Demandes",
                newName: "IX_Demandes_ValidateurId2");

            migrationBuilder.RenameIndex(
                name: "IX_Demandes_ValidateurCFOId",
                table: "Demandes",
                newName: "IX_Demandes_ValidateurId1");

            migrationBuilder.RenameIndex(
                name: "IX_Demandes_Code",
                table: "Demandes",
                newName: "IX_Demandes_Num");

            migrationBuilder.AddColumn<bool>(
                name: "IsValidateur1Validated",
                table: "Demandes",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidateur2Validated",
                table: "Demandes",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurId1",
                table: "Demandes",
                column: "ValidateurId1",
                principalTable: "Validateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Validateurs_ValidateurId2",
                table: "Demandes",
                column: "ValidateurId2",
                principalTable: "Validateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
