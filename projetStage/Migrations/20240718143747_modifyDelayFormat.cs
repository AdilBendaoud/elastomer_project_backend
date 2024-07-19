using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class modifyDelayFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Delay",
                table: "DevisItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "Delay",
                table: "DevisItems",
                type: "date",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
