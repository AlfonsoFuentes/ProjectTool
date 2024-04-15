using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SapAdjust : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SapAdjusts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualSap = table.Column<double>(type: "float", nullable: false),
                    CommitmentSap = table.Column<double>(type: "float", nullable: false),
                    PotencialSap = table.Column<double>(type: "float", nullable: false),
                    ActualSoftware = table.Column<double>(type: "float", nullable: false),
                    CommitmentSoftware = table.Column<double>(type: "float", nullable: false),
                    PotencialSoftware = table.Column<double>(type: "float", nullable: false),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MWOId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapAdjusts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SapAdjusts_MWOs_MWOId",
                        column: x => x.MWOId,
                        principalTable: "MWOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SapAdjusts_MWOId",
                table: "SapAdjusts",
                column: "MWOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SapAdjusts");
        }
    }
}
