using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class DataConversionColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationReference",
                schema: "academisation",
                table: "ConversionApplication",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsApplicationId",
                schema: "academisation",
                table: "ConversionApplication",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsSchoolLoanId",
                schema: "academisation",
                table: "ApplicationSchoolLoan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsSchoolLeaseId",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsApplyingSchoolId",
                schema: "academisation",
                table: "ApplicationSchool",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicsKeyPersonId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationReference",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropColumn(
                name: "DynamicsApplicationId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropColumn(
                name: "DynamicsSchoolLoanId",
                schema: "academisation",
                table: "ApplicationSchoolLoan");

            migrationBuilder.DropColumn(
                name: "DynamicsSchoolLeaseId",
                schema: "academisation",
                table: "ApplicationSchoolLease");

            migrationBuilder.DropColumn(
                name: "DynamicsApplyingSchoolId",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropColumn(
                name: "DynamicsKeyPersonId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");
        }
    }
}
