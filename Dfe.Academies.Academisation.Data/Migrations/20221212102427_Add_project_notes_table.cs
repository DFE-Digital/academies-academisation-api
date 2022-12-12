using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class Add_project_notes_table : Migration
    {
		private const string Schema = "academisation";
		private const string TableName = "ProjectNotes";

		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: Schema);

			migrationBuilder.CreateTable(
				name: TableName,
				schema: Schema,
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
					Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
					ProjectId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey($"PK_{TableName}_Id", x => x.Id);
					table.ForeignKey(name: $"FK_{TableName}_Project_Id",
						column: x => x.ProjectId,
						principalSchema: Schema,
						principalTable: "Project",
						principalColumn: "Id"
					);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: TableName, schema: Schema);
		}
    }
}
