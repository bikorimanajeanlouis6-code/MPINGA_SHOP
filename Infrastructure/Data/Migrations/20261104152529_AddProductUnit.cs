using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "pcs");

            migrationBuilder.Sql(
                """
                UPDATE product
                SET Unit = CASE
                    WHEN LOWER(LTRIM(RTRIM(stock.Unit))) IN ('kg', 'kgs', 'kilogram', 'kilograms') THEN 'kg'
                    WHEN LOWER(LTRIM(RTRIM(stock.Unit))) IN ('pc', 'pcs', 'piece', 'pieces', 'pieces (pcs)') THEN 'pcs'
                    ELSE 'pcs'
                END
                FROM Products AS product
                OUTER APPLY (
                    SELECT TOP (1) Unit
                    FROM Stocks
                    WHERE Stocks.ProductId = product.Id
                    ORDER BY Stocks.DateAdded DESC, Stocks.Id DESC
                ) AS stock;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Products");
        }
    }
}
