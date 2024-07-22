using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class FK_ReferenceProjectGroupInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                schema: "academisation",
                table: "ProjectGroups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ProjectGroupId",
                schema: "academisation",
                table: "Project",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_ProjectGroups_ProjectGroupId",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ProjectGroupId",
                schema: "academisation",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectGroupId",
                schema: "academisation",
                table: "Project");

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                schema: "academisation",
                table: "ProjectGroups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
