using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFormatFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "SapAdjusts");

            migrationBuilder.AddColumn<string>(
                name: "ImageDataFile",
                table: "SapAdjusts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDataFile",
                table: "SapAdjusts");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "SapAdjusts",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
