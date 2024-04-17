using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolCurrentFinancialYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentFinancialYearCapitalCarryForward",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentFinancialYearCapitalCarryForwardExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentFinancialYearCapitalCarryForwardFileLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentFinancialYearCapitalCarryForwardStatus",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentFinancialYearEndDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentFinancialYearRevenue",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentFinancialYearRevenueStatus",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentFinancialYearRevenueStatusExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentFinancialYearRevenueStatusFileLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearCapitalCarryForward",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearCapitalCarryForwardExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearCapitalCarryForwardFileLink",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearCapitalCarryForwardStatus",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearEndDate",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearRevenue",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearRevenueStatus",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearRevenueStatusExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "CurrentFinancialYearRevenueStatusFileLink",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
