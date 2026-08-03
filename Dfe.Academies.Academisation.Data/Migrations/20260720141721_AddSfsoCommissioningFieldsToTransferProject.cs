using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSfsoCommissioningFieldsToTransferProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SfsoCommissioningOverview",
                schema: "academisation",
                table: "TransferProject",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SfsoCommissioningRequestedDate",
                schema: "academisation",
                table: "TransferProject",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SfsoCommissioningOverview",
                schema: "academisation",
                table: "TransferProject");

            migrationBuilder.DropColumn(
                name: "SfsoCommissioningRequestedDate",
                schema: "academisation",
                table: "TransferProject");
        }
    }
}
