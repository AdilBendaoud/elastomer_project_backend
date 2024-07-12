using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class demandeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ValidatedByCOOAt",
                table: "Demandes",
                newName: "ValidatedOrRejectedByCOOAt");

            migrationBuilder.RenameColumn(
                name: "ValidatedByCFOAt",
                table: "Demandes",
                newName: "ValidatedOrRejectedByCFOAt");

            migrationBuilder.AlterColumn<string>(
                name: "CommentCOO",
                table: "Demandes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CommentCFO",
                table: "Demandes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsValidateurCFORejected",
                table: "Demandes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValidateurCOORejected",
                table: "Demandes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValidateurCFORejected",
                table: "Demandes");

            migrationBuilder.DropColumn(
                name: "IsValidateurCOORejected",
                table: "Demandes");

            migrationBuilder.RenameColumn(
                name: "ValidatedOrRejectedByCOOAt",
                table: "Demandes",
                newName: "ValidatedByCOOAt");

            migrationBuilder.RenameColumn(
                name: "ValidatedOrRejectedByCFOAt",
                table: "Demandes",
                newName: "ValidatedByCFOAt");

            migrationBuilder.UpdateData(
                table: "Demandes",
                keyColumn: "CommentCOO",
                keyValue: null,
                column: "CommentCOO",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CommentCOO",
                table: "Demandes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Demandes",
                keyColumn: "CommentCFO",
                keyValue: null,
                column: "CommentCFO",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CommentCFO",
                table: "Demandes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
