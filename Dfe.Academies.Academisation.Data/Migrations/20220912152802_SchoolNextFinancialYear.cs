using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolNextFinancialYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NextFinancialYearCapitalCarryForward",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextFinancialYearCapitalCarryForwardExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextFinancialYearCapitalCarryForwardFileLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextFinancialYearCapitalCarryForwardStatus",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextFinancialYearEndDate",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NextFinancialYearRevenue",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NextFinancialYearRevenueStatus",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextFinancialYearRevenueStatusExplained",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextFinancialYearRevenueStatusFileLink",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextFinancialYearCapitalCarryForward",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearCapitalCarryForwardExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearCapitalCarryForwardFileLink",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearCapitalCarryForwardStatus",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearEndDate",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearRevenue",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearRevenueStatus",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearRevenueStatusExplained",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "NextFinancialYearRevenueStatusFileLink",
                schema: "academisation",
                table: "ApplicationSchool");
        }
    }
}
