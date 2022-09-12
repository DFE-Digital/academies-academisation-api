using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.OutsideData;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class Project : IProject
{
	private Project(ProjectDetails projectDetails)
	{
		Details = projectDetails;
	}

	/// <summary>
	/// This is the persistence constructor, only use from the data layer
	/// </summary>
	public Project(int id, ProjectDetails projectDetails)
	{
		Id = id;
		Details = projectDetails;
	}

	public int Id { get; }

	public ProjectDetails Details { get; }

	public static CreateResult<IProject> Create(IApplication application, EstablishmentDetails establishmentDetails, MisEstablishmentDetails misEstablishmentDetails)
	{
		if (application.ApplicationType != Core.ApplicationAggregate.ApplicationType.JoinAMat)
		{
			return new CreateValidationErrorResult<IProject>(
				new List<ValidationError>
				{
					new ValidationError("ApplicationStatus", "Only projects of type JoinAMat are supported")
				});
		}

		var school = application.Schools.Single().Details;

		var projectDetails = new ProjectDetails(
			school.Urn,
			misEstablishmentDetails.Laestab
		)
		{ 
			UkPrn = establishmentDetails.Ukprn,
			//LocalAuthority = school.LocalAuthority.LocalAuthorityName,
			//ApplicationReferenceNumber = application.ApplicationId
			ProjectStatus = "Converter Pre-AO (C)",
			//ApplicationReceivedDate = application.ApplicationSubmittedOn
			OpeningDate = DateTime.Today.AddMonths(6),
			//TrustReferenceNumber = application.ExistingTrust.ReferenceNumber
			//NameOfTrust = application.ExistingTrust.TrustName
			AcademyTypeAndRoute = "Converter",
			ProposedAcademyOpeningDate = school.ConversionTargetDate,
			ConversionSupportGrantAmount = 25000,
			PublishedAdmissionNumber = school.CapacityPublishedAdmissionsNumber.ToString(),
			PartOfPfiScheme = ToYesNoString(school.LandAndBuildings.PartOfPfiScheme),
			//FinancialDeficit = ToYesNoString(school.SchoolCFYCapitalIsDeficit),
			//RationaleForTrust = school.SchoolConversionReasonsForJoining,
			//EqualitiesImpactAssessmentConsidered = ToYesNoString(school.SchoolAdEqualitiesImpactAssessment),
			//SponsorName = application.SponsorName,
			//SponsorReferenceNumber = application.SponsorReferenceNumber,
			//RevenueCarryForwardAtEndMarchCurrentYear = school.SchoolCFYRevenue.ConvertDeficitAmountToNegativeValue(school.SchoolCFYRevenueIsDeficit),
			//ProjectedRevenueBalanceAtEndMarchNextYear = school.SchoolNFYRevenue.ConvertDeficitAmountToNegativeValue(school.SchoolNFYRevenueIsDeficit),
			//CapitalCarryForwardAtEndMarchCurrentYear = school.SchoolCFYCapitalForward.ConvertDeficitAmountToNegativeValue(school.SchoolCFYCapitalIsDeficit),
			//CapitalCarryForwardAtEndMarchNextYear = school.SchoolNFYCapitalForward.ConvertDeficitAmountToNegativeValue(school.SchoolNFYCapitalIsDeficit),
			YearOneProjectedPupilNumbers = school.ProjectedPupilNumbersYear1,
			YearTwoProjectedPupilNumbers = school.ProjectedPupilNumbersYear2,
			YearThreeProjectedPupilNumbers	= school.ProjectedPupilNumbersYear3
		};

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}

	private static string ToYesNoString(bool? value)
	{
		if (!value.HasValue) return string.Empty;
		return value == true ? "Yes" : "No";
	}
}
