using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Remove_LegalReqirements_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalRequirement",
                schema: "academisation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalRequirement",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HadConsultation = table.Column<int>(type: "int", nullable: true),
                    HasDioceseConsented = table.Column<int>(type: "int", nullable: true),
                    HasFoundationConsented = table.Column<int>(type: "int", nullable: true),
                    HaveProvidedResolution = table.Column<int>(type: "int", nullable: true),
                    IsSectionComplete = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalRequirement", x => x.Id);
                });
        }
    }
}
