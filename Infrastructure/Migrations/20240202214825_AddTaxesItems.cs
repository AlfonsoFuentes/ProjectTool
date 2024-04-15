using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxesItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItem_Brands_BrandId",
                table: "BudgetItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItem_MWOs_MWOId",
                table: "BudgetItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetItem",
                table: "BudgetItem");

            migrationBuilder.RenameTable(
                name: "BudgetItem",
                newName: "BudgetItems");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItem_MWOId",
                table: "BudgetItems",
                newName: "IX_BudgetItems_MWOId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItem_BrandId",
                table: "BudgetItems",
                newName: "IX_BudgetItems_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetItems",
                table: "BudgetItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TaxesItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BudgetItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SelectedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxesItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxesItems_BudgetItems_BudgetItemId",
                        column: x => x.BudgetItemId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxesItems_BudgetItems_SelectedId",
                        column: x => x.SelectedId,
                        principalTable: "BudgetItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxesItems_BudgetItemId",
                table: "TaxesItems",
                column: "BudgetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxesItems_SelectedId",
                table: "TaxesItems",
                column: "SelectedId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItems_Brands_BrandId",
                table: "BudgetItems",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItems_MWOs_MWOId",
                table: "BudgetItems",
                column: "MWOId",
                principalTable: "MWOs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItems_Brands_BrandId",
                table: "BudgetItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItems_MWOs_MWOId",
                table: "BudgetItems");

            migrationBuilder.DropTable(
                name: "TaxesItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetItems",
                table: "BudgetItems");

            migrationBuilder.RenameTable(
                name: "BudgetItems",
                newName: "BudgetItem");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItems_MWOId",
                table: "BudgetItem",
                newName: "IX_BudgetItem_MWOId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItems_BrandId",
                table: "BudgetItem",
                newName: "IX_BudgetItem_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetItem",
                table: "BudgetItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItem_Brands_BrandId",
                table: "BudgetItem",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItem_MWOs_MWOId",
                table: "BudgetItem",
                column: "MWOId",
                principalTable: "MWOs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
