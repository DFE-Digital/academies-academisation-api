using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixcasingofapprovedwithconditions : Migration
    {
	    /// <inheritdoc />
	    protected override void Up(MigrationBuilder migrationBuilder)
	    {
		    migrationBuilder.Sql("UPDATE [academisation].[Project] SET ProjectStatus = 'Approved with conditions' WHERE ProjectStatus COLLATE SQL_Latin1_General_CP1_CS_AS = 'APPROVED WITH CONDITIONS'");
	    }

	    /// <inheritdoc />
	    protected override void Down(MigrationBuilder migrationBuilder)
	    {
		    migrationBuilder.Sql("UPDATE [academisation].[Project] SET ProjectStatus = 'APPROVED WITH CONDITIONS' WHERE ProjectStatus COLLATE SQL_Latin1_General_CP1_CS_AS = 'Approved with conditions'");
	    }
    }
}
