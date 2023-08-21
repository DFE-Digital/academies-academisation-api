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
            // 1. Create a temporary column.
            migrationBuilder.AddColumn<string>(
                name: "TempColumn",
                schema: "academisation",
                table: "Project",
                nullable: true);

            // 2. Combine and update the data from the two columns into the temporary column.
            migrationBuilder.Sql(@"
			UPDATE academisation.Project
			SET TempColumn = CONCAT(MemberOfParliamentName, ' ', MemberOfParliamentParty)
			");

            // 3. Rename MemberOfParliamentParty to MemberOfParliamentNameAndParty
            migrationBuilder.RenameColumn(
                name: "MemberOfParliamentParty",
                schema: "academisation",
                table: "Project",
                newName: "MemberOfParliamentNameAndParty");

            // 4. Transfer data from TempColumn to MemberOfParliamentNameAndParty
            migrationBuilder.Sql(@"
			UPDATE academisation.Project
			SET MemberOfParliamentNameAndParty = TempColumn
			");

            // 5. Drop the temporary column and the MemberOfParliamentName column.
            migrationBuilder.DropColumn(
                name: "TempColumn",
                schema: "academisation",
                table: "Project");
            migrationBuilder.DropColumn(
                name: "MemberOfParliamentName",
                schema: "academisation",
                table: "Project");

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
