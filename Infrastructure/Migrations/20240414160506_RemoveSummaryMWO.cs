using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSummaryMWO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCapital",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "ActualExpenses",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "Capital",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "CommitmentCapital",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "CommitmentExpenses",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "Expenses",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "PotentialCommitmentCapital",
                table: "MWOs");

            migrationBuilder.DropColumn(
                name: "PotentialCommitmentExpenses",
                table: "MWOs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ActualCapital",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActualExpenses",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Capital",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CommitmentCapital",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CommitmentExpenses",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Expenses",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PotentialCommitmentCapital",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PotentialCommitmentExpenses",
                table: "MWOs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
