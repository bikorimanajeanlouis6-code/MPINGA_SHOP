using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReplenishmentPaymentLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReplenishmentId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReplenishmentId",
                table: "Payments",
                column: "ReplenishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Replenishments_ReplenishmentId",
                table: "Payments",
                column: "ReplenishmentId",
                principalTable: "Replenishments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Replenishments_ReplenishmentId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ReplenishmentId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ReplenishmentId",
                table: "Payments");
        }
    }
}
