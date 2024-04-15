using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurchaseOrderMWO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "POApproveddDate",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "POCreatedDate",
                table: "PurchaseOrders",
                newName: "POClosedDate");

            migrationBuilder.RenameColumn(
                name: "POCloseddDate",
                table: "PurchaseOrders",
                newName: "POApprovedDate");

            migrationBuilder.AddColumn<string>(
                name: "CostCenter",
                table: "MWOs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MWONumber",
                table: "MWOs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostCenter",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "MWONumber",
                table: "MWOs");

            migrationBuilder.RenameColumn(
                name: "POClosedDate",
                table: "PurchaseOrders",
                newName: "POCreatedDate");

            migrationBuilder.RenameColumn(
                name: "POApprovedDate",
                table: "PurchaseOrders",
                newName: "POCloseddDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "POApproveddDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true);
        }
    }
}
