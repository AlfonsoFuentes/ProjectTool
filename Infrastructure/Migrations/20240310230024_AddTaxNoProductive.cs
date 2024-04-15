using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxNoProductive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTaxNoProductive",
                table: "PurchaseOrders",
                newName: "IsTaxEditable");

            migrationBuilder.AddColumn<bool>(
                name: "IsTaxNoProductive",
                table: "PurchaseOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTaxNoProductive",
                table: "PurchaseOrderItems");

            migrationBuilder.RenameColumn(
                name: "IsTaxEditable",
                table: "PurchaseOrders",
                newName: "IsTaxNoProductive");
        }
    }
}
