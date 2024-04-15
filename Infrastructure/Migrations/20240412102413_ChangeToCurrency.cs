using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actual",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "POValueUSD",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Actual",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "POValueUSD",
                table: "PurchaseOrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Actual",
                table: "PurchaseOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "POValueUSD",
                table: "PurchaseOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Actual",
                table: "PurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "POValueUSD",
                table: "PurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
