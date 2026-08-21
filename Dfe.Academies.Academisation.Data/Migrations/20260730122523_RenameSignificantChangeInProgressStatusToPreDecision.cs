using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameSignificantChangeInProgressStatusToPreDecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [academisation].[SignificantChangeProject] SET [Status] = 'PreDecision' WHERE [Status] = 'InProgress';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [academisation].[SignificantChangeProject] SET [Status] = 'InProgress' WHERE [Status] = 'PreDecision';");
        }
    }
}
