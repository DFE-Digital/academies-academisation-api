using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class ApplicationReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationReference",
                schema: "academisation",
                table: "ConversionApplication",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "'A2B_' + CAST([Id] AS NVARCHAR(255))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationReference",
                schema: "academisation",
                table: "ConversionApplication",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "'A2B_' + CAST([Id] AS NVARCHAR(255))");
        }
    }
}
