using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    /// <inheritdoc />
    public partial class transfer_project : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferProject",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "10003000, 1"),
                    Urn = table.Column<int>(type: "int", nullable: false),
                    ProjectReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutgoingTrustUkprn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhoInitiatedTheTransfer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RddOrEsfaIntervention = table.Column<bool>(type: "bit", nullable: true),
                    RddOrEsfaInterventionDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfTransfer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherTransferTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferFirstDiscussed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TargetDateForTransfer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HtbDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasTransferFirstDiscussedDate = table.Column<bool>(type: "bit", nullable: true),
                    HasTargetDateForTransfer = table.Column<bool>(type: "bit", nullable: true),
                    HasHtbDate = table.Column<bool>(type: "bit", nullable: true),
                    ProjectRationale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrustSponsorRationale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnyRisks = table.Column<bool>(type: "bit", nullable: true),
                    HighProfileShouldBeConsidered = table.Column<bool>(type: "bit", nullable: true),
                    HighProfileFurtherSpecification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplexLandAndBuildingShouldBeConsidered = table.Column<bool>(type: "bit", nullable: true),
                    ComplexLandAndBuildingFurtherSpecification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinanceAndDebtShouldBeConsidered = table.Column<bool>(type: "bit", nullable: true),
                    FinanceAndDebtFurtherSpecification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherRisksShouldBeConsidered = table.Column<bool>(type: "bit", nullable: true),
                    EqualitiesImpactAssessmentConsidered = table.Column<bool>(type: "bit", nullable: true),
                    OtherRisksFurtherSpecification = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true),
                    OtherBenefitValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomingTrustAgreement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiocesanConsent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutgoingTrustConsent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalRequirementsSectionIsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    FeatureSectionIsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    BenefitsSectionIsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    RationaleSectionIsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    AssignedUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedUserEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferProject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntendedTransferBenefit",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferProjectId = table.Column<int>(type: "int", nullable: false),
                    SelectedBenefit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntendedTransferBenefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntendedTransferBenefit_TransferProject_TransferProjectId",
                        column: x => x.TransferProjectId,
                        principalSchema: "academisation",
                        principalTable: "TransferProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferringAcademy",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferProjectId = table.Column<int>(type: "int", nullable: false),
                    OutgoingAcademyUkprn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IncomingTrustUkprn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PupilNumbersAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatestOfstedReportAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyStage2PerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyStage4PerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyStage5PerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferringAcademy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferringAcademy_TransferProject_TransferProjectId",
                        column: x => x.TransferProjectId,
                        principalSchema: "academisation",
                        principalTable: "TransferProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntendedTransferBenefit_TransferProjectId",
                schema: "academisation",
                table: "IntendedTransferBenefit",
                column: "TransferProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferringAcademy_TransferProjectId",
                schema: "academisation",
                table: "TransferringAcademy",
                column: "TransferProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntendedTransferBenefit",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "TransferringAcademy",
                schema: "academisation");

            migrationBuilder.DropTable(
                name: "TransferProject",
                schema: "academisation");
        }
    }
}
