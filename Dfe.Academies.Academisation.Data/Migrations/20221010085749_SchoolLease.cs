using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class SchoolLease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.CreateTable(
		        name: "ApplicationSchoolLease",
		        schema: "academisation",
		        columns: table => new
		        {
			        Id = table.Column<int>(type: "int", nullable: false)
				        .Annotation("SqlServer:Identity", "1, 1"),
			        LeaseTerm = table.Column<int>(type: "int", nullable: false),
			        RepaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
			        InterestRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
			        PaymentsToDate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
			        Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
			        ValueOfAssets = table.Column<string>(type: "nvarchar(max)", nullable: false),
			        ResponsibleForAssets = table.Column<string>(type: "nvarchar(max)", nullable: false),
			        ApplicationSchoolId = table.Column<int>(type: "int", nullable: false),
			        CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
			        LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
		        },
		        constraints: table =>
		        {
			        table.PrimaryKey("PK_ApplicationSchoolLease", x => x.Id);
			        table.ForeignKey(
				        name: "FK_ApplicationSchoolLease_ApplicationSchool_ApplicationSchoolId",
				        column: x => x.ApplicationSchoolId,
				        principalSchema: "academisation",
				        principalTable: "ApplicationSchool",
				        principalColumn: "Id");
		        });

	        migrationBuilder.CreateIndex(
		        name: "IX_ApplicationSchoolLease_ApplicationSchoolId",
		        schema: "academisation",
		        table: "ApplicationSchoolLease",
		        column: "ApplicationSchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationSchoolLease_ApplicationSchool_ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLease");

            migrationBuilder.AlterColumn<string>(
                name: "LeaseTerm",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "int",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
	            name: "PaymentsToDate",
	            schema: "academisation",
	            table: "ApplicationSchoolLease",
	            type: "decimal(18,2)",
	            nullable: false,
	            defaultValue: 0m,
	            oldClrType: typeof(decimal),
	            oldType: "decimal(18,2)",
	            oldNullable: true);
            
            migrationBuilder.AlterColumn<decimal>(
                name: "RepaymentAmount",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ValueOfAssets",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResponsibleForAssets",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationSchoolId",
                schema: "academisation",
                table: "ApplicationSchoolLease",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
