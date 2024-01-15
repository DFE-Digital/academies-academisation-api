using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedResidentialPlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Details_NumberOfFundedResidentialPlaces",
                schema: "academisation",
                table: "Project",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Details_NumberOfResidentialPlaces",
                schema: "academisation",
                table: "Project",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details_NumberOfFundedResidentialPlaces",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Details_NumberOfResidentialPlaces",
                schema: "academisation",
                table: "Project");
        }
    }
}
