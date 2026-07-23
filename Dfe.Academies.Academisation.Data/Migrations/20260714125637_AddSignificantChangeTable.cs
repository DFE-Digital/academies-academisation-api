using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSignificantChangeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignificantChangeProject",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Urn = table.Column<int>(type: "int", nullable: false),
                    Tier = table.Column<byte>(type: "tinyint", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssignedUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedUserEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrustName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrustUkprn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeOfSignificantChange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignificantChangeProject", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignificantChangeProject",
                schema: "academisation");
        }
    }
}
