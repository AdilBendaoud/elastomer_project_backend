using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class demandeIdAchteurFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Acheteurs_AcheteurId",
                table: "Demandes");

            migrationBuilder.AlterColumn<int>(
                name: "ValidateurCOOId",
                table: "Demandes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ValidateurCFOId",
                table: "Demandes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AcheteurId",
                table: "Demandes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Acheteurs_AcheteurId",
                table: "Demandes",
                column: "AcheteurId",
                principalTable: "Acheteurs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandes_Acheteurs_AcheteurId",
                table: "Demandes");

            migrationBuilder.AlterColumn<int>(
                name: "ValidateurCOOId",
                table: "Demandes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ValidateurCFOId",
                table: "Demandes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AcheteurId",
                table: "Demandes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandes_Acheteurs_AcheteurId",
                table: "Demandes",
                column: "AcheteurId",
                principalTable: "Acheteurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
