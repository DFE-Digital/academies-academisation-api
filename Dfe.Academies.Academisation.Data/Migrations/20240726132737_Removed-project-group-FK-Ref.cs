using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedprojectgroupFKRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_ProjectGroups_ProjectGroupId",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectGroupId",
                schema: "academisation",
                table: "Project");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectGroupId",
                schema: "academisation",
                table: "Project",
                column: "ProjectGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_ProjectGroups_ProjectGroupId",
                schema: "academisation",
                table: "Project",
                column: "ProjectGroupId",
                principalSchema: "academisation",
                principalTable: "ProjectGroups",
                principalColumn: "Id");
        }
    }
}
