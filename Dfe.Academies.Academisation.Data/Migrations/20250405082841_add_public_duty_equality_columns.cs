using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_public_duty_equality_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicEqualityDutyImpact",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicEqualityDutyReduceImpactReason",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PublicEqualityDutySectionComplete",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicEqualityDutyImpact",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PublicEqualityDutyReduceImpactReason",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "PublicEqualityDutySectionComplete",
                schema: "academisation",
                table: "Project");
        }
    }
}
