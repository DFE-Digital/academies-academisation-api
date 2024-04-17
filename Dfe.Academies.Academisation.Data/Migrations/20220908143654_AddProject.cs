using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.Academies.Academisation.Data.Migrations
{
    public partial class AddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                schema: "academisation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Urn = table.Column<int>(type: "int", nullable: false),
                    Laestab = table.Column<int>(type: "int", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalAuthority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UkPrn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HeadTeacherBoardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaselineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocalAuthorityInformationTemplateSentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocalAuthorityInformationTemplateReturnedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LocalAuthorityInformationTemplateComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalAuthorityInformationTemplateLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocalAuthorityInformationTemplateSectionComplete = table.Column<bool>(type: "bit", nullable: true),
                    RecommendationForProject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClearedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademyOrderRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousHeadTeacherBoardDateQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousHeadTeacherBoardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PreviousHeadTeacherBoardLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrustReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfTrust = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademyTypeAndRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProposedAcademyOpeningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SchoolAndTrustInformationSectionComplete = table.Column<bool>(type: "bit", nullable: true),
                    ConversionSupportGrantAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConversionSupportGrantChangeReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedAdmissionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfPfiScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViabilityIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialDeficit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistanceFromSchoolToTrustHeadquarters = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DistanceFromSchoolToTrustHeadquartersAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberOfParliamentParty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberOfParliamentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralInformationSectionComplete = table.Column<bool>(type: "bit", nullable: true),
                    SchoolPerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RationaleForProject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RationaleForTrust = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RationaleSectionComplete = table.Column<bool>(type: "bit", nullable: true),
                    RisksAndIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EqualitiesImpactAssessmentConsidered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RisksAndIssuesSectionComplete = table.Column<bool>(type: "bit", nullable: true),
                    RevenueCarryForwardAtEndMarchCurrentYear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectedRevenueBalanceAtEndMarchNextYear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CapitalCarryForwardAtEndMarchCurrentYear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CapitalCarryForwardAtEndMarchNextYear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SchoolBudgetInformationAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolBudgetInformationSectionComplete = table.Column<bool>(type: "bit", nullable: true),
                    CurrentYearCapacity = table.Column<int>(type: "int", nullable: true),
                    CurrentYearPupilNumbers = table.Column<int>(type: "int", nullable: true),
                    YearOneProjectedCapacity = table.Column<int>(type: "int", nullable: true),
                    YearOneProjectedPupilNumbers = table.Column<int>(type: "int", nullable: true),
                    YearTwoProjectedCapacity = table.Column<int>(type: "int", nullable: true),
                    YearTwoProjectedPupilNumbers = table.Column<int>(type: "int", nullable: true),
                    YearThreeProjectedCapacity = table.Column<int>(type: "int", nullable: true),
                    YearThreeProjectedPupilNumbers = table.Column<int>(type: "int", nullable: true),
                    SchoolPupilForecastsAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyStage2PerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyStage4PerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyStage5PerformanceAdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Upin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewAcademyUrn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project",
                schema: "academisation");
        }
    }
}
