using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePercentageFromMWO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageAssetNoProductive",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "PercentageContingency",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "PercentageEngineering",
                table: "MWOs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PercentageAssetNoProductive",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageContingency",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PercentageEngineering",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
