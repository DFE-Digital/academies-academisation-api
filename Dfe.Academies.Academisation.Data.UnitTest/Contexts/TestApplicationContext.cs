using System;
using System.Collections.Generic;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public class TestApplicationContext : TestAcademisationContext
{
	private readonly Fixture _fixture = new();

	public TestApplicationContext()
	{
		Seed();
	}

	protected override void SeedData()
	{
		using var context = CreateContext();
		context.Database.EnsureCreated();

		var timestamp = DateTime.UtcNow;

		var seed = new List<Application>
		{
			BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.JoinAMat, timestamp, $"A2B_{1}"),
			BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.JoinAMat, timestamp, $"A2B_{2}"),
			BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.JoinAMat, timestamp, $"A2B_{3}"),
		};

		context.AddRange(seed);
		context.SaveChanges();
	}

	private Application BuildApplication(ApplicationStatus applicationStatus, int numberOfSchools,
		ApplicationType? type, DateTime createDateTime, string applicationRef)
	{
		var schools = new List<School>();

		for (int i = 0; i < numberOfSchools; i++)
		{
			var ms = _fixture.Create<School>();

			schools.Add(new School(ms.Id, ms.TrustBenefitDetails, ms.OfstedInspectionDetails, ms.SafeguardingDetails,
				ms.LocalAuthorityReorganisationDetails, ms.LocalAuthorityClosurePlanDetails, ms.DioceseName,
				ms.DioceseFolderIdentifier, ms.PartOfFederation, ms.FoundationTrustOrBodyName,
				ms.FoundationConsentFolderIdentifier, ms.ExemptionEndDate, ms.MainFeederSchools, ms.ResolutionConsentFolderIdentifier, ms.ProtectedCharacteristics, ms.FurtherInformation, 
				new SchoolDetails(ms.Details.Urn, ms.Details.ProposedNewSchoolName!, ms.Details.LandAndBuildings,
					ms.Details.PreviousFinancialYear, ms.Details.CurrentFinancialYear, ms.Details.NextFinancialYear,
					ms.Details.ContactHeadName, $"{ms.Details.ContactHeadEmail}@test.com", ms.Details.ContactHeadTel,
					ms.Details.ContactChairName, $"{ms.Details.ContactChairEmail}@test.com", ms.Details.ContactChairTel,
					ms.Details.ContactRole, ms.Details.MainContactOtherName, $"{ms.Details.MainContactOtherEmail}@test.com",
					ms.Details.MainContactOtherTelephone, ms.Details.MainContactOtherRole,
					ms.Details.ApproverContactName, $"{ms.Details.ApproverContactEmail}@test.com",
					ms.Details.ConversionTargetDateSpecified, ms.Details.ConversionTargetDate,
					ms.Details.ConversionTargetDateExplained, ms.Details.ConversionChangeNamePlanned,
					ms.Details.ProposedNewSchoolName, ms.Details.ApplicationJoinTrustReason,
					ms.Details.ProjectedPupilNumbersYear1, ms.Details.ProjectedPupilNumbersYear1,
					ms.Details.ProjectedPupilNumbersYear1, ms.Details.CapacityAssumptions,
					ms.Details.CapacityPublishedAdmissionsNumber, ms.Details.SchoolSupportGrantFundsPaidTo,
					ms.Details.ConfirmPaySupportGrantToSchool, ms.Details.SchoolHasConsultedStakeholders,
					ms.Details.SchoolPlanToConsultStakeholders, ms.Details.FinanceOngoingInvestigations,
					ms.Details.FinancialInvestigationsExplain, ms.Details.FinancialInvestigationsTrustAware,
					ms.Details.DeclarationBodyAgree, ms.Details.DeclarationIAmTheChairOrHeadteacher,
					ms.Details.DeclarationSignedByName, ms.Details.SchoolConversionReasonsForJoining),
				ms.Loans, ms.Leases, ms.HasLoans, ms.HasLeases));

			
		}

		Application application = new(
			_fixture.Create<int>(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			type ?? _fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			schools,
			JoinTrust.Create(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.Create<string>(),_fixture.Create<ChangesToTrust>(),
				_fixture.Create<string>(), _fixture.Create<bool>(), _fixture.Create<string>()),
			null, null, applicationRef);

		return application;
	}
}
