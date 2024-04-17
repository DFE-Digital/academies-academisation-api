using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class removeapplicationstaterefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFormTrustKeyPerson_ApplicationFormTrust_ApplicationFormTrustId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFormTrustKeyPersonRole_ApplicationFormTrustKeyPerson_ApplicationFormTrustKeyPersonRoleId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPersonRole");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchool_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchoolLease_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLease");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchoolLoan_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLoan");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversionApplicationContributor_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ConversionApplicationContributor");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.RenameColumn(
                name: "LocalAuthorityReoganisationDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "LocalAuthorityReorganisationDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "DynamicsKeyPersonId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "FormTrustId",
                unique: true,
                filter: "[FormTrustId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "JoinTrustId",
                unique: true,
                filter: "[JoinTrustId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFormTrustKeyPerson_ApplicationFormTrust_ApplicationFormTrustId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                column: "ApplicationFormTrustId",
                principalSchema: "academisation",
                principalTable: "ApplicationFormTrust",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFormTrustKeyPersonRole_ApplicationFormTrustKeyPerson_ApplicationFormTrustKeyPersonRoleId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPersonRole",
                column: "ApplicationFormTrustKeyPersonRoleId",
                principalSchema: "academisation",
                principalTable: "ApplicationFormTrustKeyPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationSchool_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ApplicationSchool",
                column: "ConversionApplicationId",
                principalSchema: "academisation",
                principalTable: "ConversionApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationSchoolLease_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                column: "ApplicationSchoolId",
                principalSchema: "academisation",
                principalTable: "ApplicationSchool",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationSchoolLoan_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLoan",
                column: "ApplicationSchoolId",
                principalSchema: "academisation",
                principalTable: "ApplicationSchool",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionApplicationContributor_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ConversionApplicationContributor",
                column: "ConversionApplicationId",
                principalSchema: "academisation",
                principalTable: "ConversionApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFormTrustKeyPerson_ApplicationFormTrust_ApplicationFormTrustId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFormTrustKeyPersonRole_ApplicationFormTrustKeyPerson_ApplicationFormTrustKeyPersonRoleId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPersonRole");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchool_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ApplicationSchool");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchoolLease_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLease");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchoolLoan_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLoan");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversionApplicationContributor_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ConversionApplicationContributor");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.DropIndex(
                name: "IX_ConversionApplication_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication");

            migrationBuilder.RenameColumn(
                name: "LocalAuthorityReorganisationDetails",
                schema: "academisation",
                table: "ApplicationSchool",
                newName: "LocalAuthorityReoganisationDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "DynamicsKeyPersonId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_FormTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "FormTrustId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversionApplication_JoinTrustId",
                schema: "academisation",
                table: "ConversionApplication",
                column: "JoinTrustId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFormTrustKeyPerson_ApplicationFormTrust_ApplicationFormTrustId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPerson",
                column: "ApplicationFormTrustId",
                principalSchema: "academisation",
                principalTable: "ApplicationFormTrust",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFormTrustKeyPersonRole_ApplicationFormTrustKeyPerson_ApplicationFormTrustKeyPersonRoleId",
                schema: "academisation",
                table: "ApplicationFormTrustKeyPersonRole",
                column: "ApplicationFormTrustKeyPersonRoleId",
                principalSchema: "academisation",
                principalTable: "ApplicationFormTrustKeyPerson",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationSchool_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ApplicationSchool",
                column: "ConversionApplicationId",
                principalSchema: "academisation",
                principalTable: "ConversionApplication",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationSchoolLease_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                column: "ApplicationSchoolId",
                principalSchema: "academisation",
                principalTable: "ApplicationSchool",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationSchoolLoan_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLoan",
                column: "ApplicationSchoolId",
                principalSchema: "academisation",
                principalTable: "ApplicationSchool",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversionApplicationContributor_ConversionApplication_ConversionApplicationId",
                schema: "academisation",
                table: "ConversionApplicationContributor",
                column: "ConversionApplicationId",
                principalSchema: "academisation",
                principalTable: "ConversionApplication",
                principalColumn: "Id");
        }
    }
}
