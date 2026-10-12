using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Sales",
                newName: "TotalConsumed");

            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "Sales",
                newName: "SaleDate");

            migrationBuilder.AddColumn<string>(
                name: "Balance",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "0");

            migrationBuilder.AddColumn<string>(
                name: "TotalPaid",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "0");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalPaid",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "TotalConsumed",
                table: "Sales",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "SaleDate",
                table: "Sales",
                newName: "DateAdded");
        }
    }
}
