using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class modifierUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedsPasswordChange",
                table: "Validateurs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsPasswordChange",
                table: "Demandeurs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsPasswordChange",
                table: "Admins",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsPasswordChange",
                table: "Acheteurs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedsPasswordChange",
                table: "Validateurs");

            migrationBuilder.DropColumn(
                name: "NeedsPasswordChange",
                table: "Demandeurs");

            migrationBuilder.DropColumn(
                name: "NeedsPasswordChange",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "NeedsPasswordChange",
                table: "Acheteurs");
        }
    }
}
