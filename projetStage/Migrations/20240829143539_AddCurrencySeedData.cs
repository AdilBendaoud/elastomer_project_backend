using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projetStage.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WESM_currencies",
                columns: new[] { "Id", "CurrencyCode", "PriceInEur" },
                values: new object[,]
                {
                    { 1, "USD", 0.92f },
                    { 2, "MAD", 0.093f },
                    { 3, "GBP", 1.17f },
                    { 4, "EUR", 1f }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WESM_currencies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WESM_currencies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WESM_currencies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WESM_currencies",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
