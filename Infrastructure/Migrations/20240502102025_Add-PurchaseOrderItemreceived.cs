using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderItemreceived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrderItemReceiveds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchaseOrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValueReceivedCurrency = table.Column<double>(type: "float", nullable: false),
                    USDCOP = table.Column<double>(type: "float", nullable: false),
                    USDEUR = table.Column<double>(type: "float", nullable: false),
                    CurrencyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItemReceiveds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItemReceiveds_PurchaseOrderItems_PurchaseOrderItemId",
                        column: x => x.PurchaseOrderItemId,
                        principalTable: "PurchaseOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItemReceiveds_PurchaseOrderItemId",
                table: "PurchaseOrderItemReceiveds",
                column: "PurchaseOrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItemReceiveds_TenantId",
                table: "PurchaseOrderItemReceiveds",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderItemReceiveds");
        }
    }
}
