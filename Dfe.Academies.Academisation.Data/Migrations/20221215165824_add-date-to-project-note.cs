﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class adddatetoprojectnote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<DateTime>(
				name: "Date",
				schema: "academisation",
				table: "ProjectNotes",
				type: "datetime2",
				nullable: true);
		}

        protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ConversionTargetDate",
				schema: "academisation",
				table: "ApplicationSchool");
		}
    }
}
