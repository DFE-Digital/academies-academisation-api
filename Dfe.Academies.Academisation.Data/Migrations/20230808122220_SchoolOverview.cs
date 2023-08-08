using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class SchoolOverview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberOfParliamentName",
                schema: "academisation",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "MemberOfParliamentParty",
                schema: "academisation",
                table: "Project",
                newName: "MemberOfParliamentNameAndParty");

            migrationBuilder.RenameColumn(
                name: "GeneralInformationSectionComplete",
                schema: "academisation",
                table: "Project",
                newName: "SchoolOverviewSectionComplete");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolOverviewSectionComplete",
                schema: "academisation",
                table: "Project",
                newName: "GeneralInformationSectionComplete");

            migrationBuilder.RenameColumn(
                name: "MemberOfParliamentNameAndParty",
                schema: "academisation",
                table: "Project",
                newName: "MemberOfParliamentParty");

            migrationBuilder.AddColumn<string>(
                name: "MemberOfParliamentName",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
