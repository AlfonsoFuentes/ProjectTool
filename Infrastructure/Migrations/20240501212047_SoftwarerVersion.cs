using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SoftwarerVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwareVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdatedSoftwareVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SoftwareVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdatedSoftwareVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdatedSoftwareVersions_AspNetUsers_AplicationUserId",
                        column: x => x.AplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpdatedSoftwareVersions_AplicationUserId",
                table: "UpdatedSoftwareVersions",
                column: "AplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdatedSoftwareVersions_TenantId",
                table: "UpdatedSoftwareVersions",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwareVersions");

            migrationBuilder.DropTable(
                name: "UpdatedSoftwareVersions");
        }
    }
}
