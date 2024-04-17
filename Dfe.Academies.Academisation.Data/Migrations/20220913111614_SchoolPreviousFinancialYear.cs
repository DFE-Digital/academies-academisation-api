using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolPreviousFinancialYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PreviousFinancialYearCapitalCarryForward",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousFinancialYearCapitalCarryForwardExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousFinancialYearCapitalCarryForwardFileLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousFinancialYearCapitalCarryForwardStatus",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousFinancialYearEndDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousFinancialYearRevenue",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousFinancialYearRevenueStatus",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousFinancialYearRevenueStatusExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousFinancialYearRevenueStatusFileLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearCapitalCarryForward",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearCapitalCarryForwardExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearCapitalCarryForwardFileLink",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearCapitalCarryForwardStatus",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearEndDate",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearRevenue",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearRevenueStatus",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearRevenueStatusExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "PreviousFinancialYearRevenueStatusFileLink",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
