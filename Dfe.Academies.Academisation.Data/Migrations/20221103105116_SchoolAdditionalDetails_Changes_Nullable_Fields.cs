using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolAdditionalDetails_Changes_Nullable_Fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationJoinTrustReason",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "SchoolSupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.AlterColumn<bool>(
                name: "PartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "MainFeederSchools",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "PartOfFederation",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MainFeederSchools",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationJoinTrustReason",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolSupportGrantFundsPaidTo",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "int",
                nullable: true);
        }
    }
}
