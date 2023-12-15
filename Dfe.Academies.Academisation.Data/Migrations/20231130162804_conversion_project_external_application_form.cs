using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class conversion_project_external_application_form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExternalApplicationFormSaved",
                schema: "academisation",
                table: "Project",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalApplicationFormUrl",
                schema: "academisation",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalApplicationFormSaved",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ExternalApplicationFormUrl",
                schema: "academisation",
                table: "Project");
        }
    }
}
