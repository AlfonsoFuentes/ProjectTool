using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "IsAlteration",
                table: "PurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "IsTaxNoProductive",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "IsDiscountApplied",
                table: "PurchaseOrders",
                newName: "IsTaxNoProductive");

            migrationBuilder.AddColumn<bool>(
                name: "IsCapitalizedSalary",
                table: "PurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCapitalizedSalary",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "IsTaxNoProductive",
                table: "PurchaseOrders",
                newName: "IsDiscountApplied");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                table: "PurchaseOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAlteration",
                table: "PurchaseOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTaxNoProductive",
                table: "PurchaseOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
