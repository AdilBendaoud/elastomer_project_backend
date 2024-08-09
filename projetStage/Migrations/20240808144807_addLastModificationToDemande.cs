using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class addLastModificationToDemande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModification",
                table: "WESM_demandes",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModification",
                table: "WESM_demandes");
        }
    }
}
