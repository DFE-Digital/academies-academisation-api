using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDecisionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ConversionProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TransferProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision");

            migrationBuilder.AlterColumn<int>(
                name: "ConversionProjectId",
                schema: "academisation",
                table: "ConversionAdvisoryBoardDecision",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
