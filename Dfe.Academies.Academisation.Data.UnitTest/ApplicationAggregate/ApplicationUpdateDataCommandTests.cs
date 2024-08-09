using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using MediatR;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ApplicationAggregate;

public class ApplicationUpdateDataCommandTests
{
	private readonly AcademisationContext _context;
	private readonly Fixture _fixture = new();
	private readonly IApplicationRepository _repo;
	private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
	private readonly IMediator _mediator;
	public ApplicationUpdateDataCommandTests()
	{
		_context = new TestApplicationContext(_mediator).CreateContext();
		_repo = new ApplicationRepository(_context, _mapper.Object);
		_fixture.Customize(new AutoMoqCustomization());
		//_fixture.Customize<Loan>(composer =>
		//	composer
		//		.With(s => s.Id, 0));
	}

	[Fact]
	public async Task RecordAlreadyExists_NoChange___LastModifiedOnUpdated()
	{
		//Arrange
		var existingApplications = await _repo.GetAllAsync();
		var existingApplication = existingApplications.FirstOrDefault();
		Assert.NotNull(existingApplication);

		var existingModDate = existingApplication.LastModifiedOn;
		var existingCreatedDate = existingApplication.CreatedOn;

		//Act
		_repo.Update(existingApplication);
		await _repo.UnitOfWork.SaveChangesAsync();
		_context.ChangeTracker.Clear();
		var updatedApplication = await _repo.GetByIdAsync(existingApplication.Id);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.Multiple(
			() => Assert.Equal(existingCreatedDate, updatedApplication.CreatedOn),
			() => Assert.NotEqual(existingModDate, updatedApplication.LastModifiedOn)
		);
	}

	[Fact]
	public async Task RecordAlreadyExists_Submit___FieldChangesPersisted()
	{
		//Arrange
		var existingApplications = await _repo.GetAllAsync();
		var existingApplication = existingApplications.FirstOrDefault();

		Assert.NotNull(existingApplication);
		var subDate = DateTime.Now;

		existingApplication.Submit(subDate);

		//Act
		_repo.Update(existingApplication);
		await _repo.UnitOfWork.SaveChangesAsync();
		_context.ChangeTracker.Clear();
		var updatedApplication = await _repo.GetByIdAsync(existingApplication.Id);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.Equal(ApplicationStatus.Submitted, updatedApplication.ApplicationStatus);
		Assert.Equal(subDate, updatedApplication.ApplicationSubmittedDate);
	}

	[Fact]
	public async Task RecordAlreadyExists_ContributorAdded___AddedContributorPersisted()
	{
		//Arrange
		var existingApplications = await _repo.GetAllAsync();
		var existingApplication = existingApplications.FirstOrDefault();

		Assert.NotNull(existingApplication);

		var addedContributorDetails = _fixture.Create<ContributorDetails>();

		existingApplication.Update(existingApplication.ApplicationType, existingApplication.ApplicationStatus,
			existingApplication.Contributors.ToDictionary(x => x.Id, x => x.Details).Append(new KeyValuePair<int, ContributorDetails>(
					0, addedContributorDetails)),
			existingApplication.Schools.Select(s =>
				new UpdateSchoolParameter(s.Id,
					s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.Safeguarding,
					s.LocalAuthorityReorganisationDetails,
					s.LocalAuthorityClosurePlanDetails,
					s.DioceseName,
					s.DioceseFolderIdentifier,
					s.PartOfFederation,
					s.FoundationTrustOrBodyName,
					s.FoundationConsentFolderIdentifier,
					s.ExemptionEndDate,
					s.MainFeederSchools,
					s.ResolutionConsentFolderIdentifier,
					s.ProtectedCharacteristics,
					s.FurtherInformation,
					s.Details,
					new List<KeyValuePair<int, LoanDetails>>(s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)))),
					new List<KeyValuePair<int, LeaseDetails>>(s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets)))),
					s.HasLoans,
					s.HasLeases)));

		//Act
		_repo.Update(existingApplication);
		await _repo.UnitOfWork.SaveChangesAsync();
		_context.ChangeTracker.Clear();
		var updatedApplication = await _repo.GetByIdAsync(existingApplication.Id);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.True(updatedApplication.Contributors.Any(x =>
			x.Details.EmailAddress == addedContributorDetails.EmailAddress &&
			x.Details.FirstName == addedContributorDetails.FirstName &&
			x.Details.LastName == addedContributorDetails.LastName &&
			x.Details.OtherRoleName == addedContributorDetails.OtherRoleName &&
			x.Details.Role == addedContributorDetails.Role));
	}

	[Fact]
	public async Task RecordAlreadyExists_ContributorRemoved___RemovedContributorPersisted()
	{
		//Arrange
		var existingApplications = await _repo.GetAllAsync();
		var existingApplication = existingApplications.FirstOrDefault();

		Assert.NotNull(existingApplication);

		var mutatedContributorList = new List<IContributor>((IEnumerable<IContributor>)existingApplication.Contributors);
		var contributorToRemove = PickRandomElement(mutatedContributorList);
		mutatedContributorList.Remove(contributorToRemove);

		existingApplication.Update(existingApplication.ApplicationType, existingApplication.ApplicationStatus,
			mutatedContributorList.ToDictionary(x => x.Id, x => x.Details),
			existingApplication.Schools.Select(s =>
				new UpdateSchoolParameter(s.Id,
					s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.Safeguarding,
					s.LocalAuthorityReorganisationDetails,
					s.LocalAuthorityClosurePlanDetails,
					s.DioceseName,
					s.DioceseFolderIdentifier,
					s.PartOfFederation,
					s.FoundationTrustOrBodyName,
					s.FoundationConsentFolderIdentifier,
					s.ExemptionEndDate,
					s.MainFeederSchools,
					s.ResolutionConsentFolderIdentifier,
					s.ProtectedCharacteristics,
					s.FurtherInformation,
					s.Details,
					new List<KeyValuePair<int, LoanDetails>>(s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)))),
					new List<KeyValuePair<int, LeaseDetails>>(s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets)))),
					s.HasLoans,
					s.HasLeases)));

		//Act
		_repo.Update(existingApplication);
		await _repo.UnitOfWork.SaveChangesAsync();
		_context.ChangeTracker.Clear();
		var updatedApplication = await _repo.GetByIdAsync(existingApplication.Id);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.False(updatedApplication.Contributors.Any(x =>
			x.Details.EmailAddress == contributorToRemove.Details.EmailAddress &&
			x.Details.FirstName == contributorToRemove.Details.FirstName &&
			x.Details.LastName == contributorToRemove.Details.LastName &&
			x.Details.OtherRoleName == contributorToRemove.Details.OtherRoleName &&
			x.Details.Role == contributorToRemove.Details.Role));
	}

	[Fact]
	public async Task RecordAlreadyExists_SchoolAdded___AddedSchoolPersisted()
	{
		//Arrange
		var existingApplications = await _repo.GetAllAsync();
		var existingApplication = existingApplications.FirstOrDefault();

		Assert.NotNull(existingApplication);

		var ms = _fixture.Create<School>();

		var addedSchool = new School(0, ms.TrustBenefitDetails, ms.OfstedInspectionDetails, ms.Safeguarding,
			ms.LocalAuthorityReorganisationDetails, ms.LocalAuthorityClosurePlanDetails, ms.DioceseName,
			ms.DioceseFolderIdentifier, ms.PartOfFederation, ms.FoundationTrustOrBodyName,
			ms.FoundationConsentFolderIdentifier, ms.ExemptionEndDate, ms.MainFeederSchools, ms.ResolutionConsentFolderIdentifier, ms.ProtectedCharacteristics, ms.FurtherInformation,
			new SchoolDetails(ms.Details.Urn, ms.Details.ProposedNewSchoolName!, ms.Details.LandAndBuildings,
				ms.Details.PreviousFinancialYear, ms.Details.CurrentFinancialYear, ms.Details.NextFinancialYear,
				ms.Details.ContactHeadName, $"{ms.Details.ContactHeadEmail}@test.com",
				ms.Details.ContactChairName, $"{ms.Details.ContactChairEmail}@test.com",
				ms.Details.ContactRole, ms.Details.MainContactOtherName, $"{ms.Details.MainContactOtherEmail}@test.com",
				ms.Details.MainContactOtherRole,
				ms.Details.ApproverContactName, $"{ms.Details.ApproverContactEmail}@test.com",
				ms.Details.ConversionTargetDateSpecified, ms.Details.ConversionTargetDate,
				ms.Details.ConversionTargetDateExplained, ms.Details.ConversionChangeNamePlanned,
				ms.Details.ProposedNewSchoolName, ms.Details.ApplicationJoinTrustReason,
				ms.Details.ProjectedPupilNumbersYear1, ms.Details.ProjectedPupilNumbersYear1,
				ms.Details.ProjectedPupilNumbersYear1, ms.Details.CapacityAssumptions,
				ms.Details.CapacityPublishedAdmissionsNumber, ms.Details.SchoolSupportGrantFundsPaidTo,
				ms.Details.ConfirmPaySupportGrantToSchool,
				ms.Details.SchoolSupportGrantJoiningInAGroup, ms.Details.SchoolSupportGrantBankDetailsProvided,
				ms.Details.SchoolsInGroup,
				ms.Details.SchoolHasConsultedStakeholders,
				ms.Details.SchoolPlanToConsultStakeholders, ms.Details.FinanceOngoingInvestigations,
				ms.Details.FinancialInvestigationsExplain, ms.Details.FinancialInvestigationsTrustAware,
				ms.Details.DeclarationBodyAgree, ms.Details.DeclarationIAmTheChairOrHeadteacher,
				ms.Details.DeclarationSignedByName, ms.Details.SchoolConversionReasonsForJoining),
			ms.Loans, ms.Leases, ms.HasLoans, ms.HasLeases);

		var schools = new List<School> { addedSchool };

		existingApplication.Update(existingApplication.ApplicationType, existingApplication.ApplicationStatus,
			existingApplication.Contributors.ToDictionary(x => x.Id, x => x.Details),
			schools.Select(s =>
				new UpdateSchoolParameter(s.Id,
					s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.Safeguarding,
					s.LocalAuthorityReorganisationDetails,
					s.LocalAuthorityClosurePlanDetails,
					s.DioceseName,
					s.DioceseFolderIdentifier,
					s.PartOfFederation,
					s.FoundationTrustOrBodyName,
					s.FoundationConsentFolderIdentifier,
					s.ExemptionEndDate,
					s.MainFeederSchools,
					s.ResolutionConsentFolderIdentifier,
					s.ProtectedCharacteristics,
					s.FurtherInformation,
					s.Details,
					new List<KeyValuePair<int, LoanDetails>>(s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)))),
					new List<KeyValuePair<int, LeaseDetails>>(s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets)))),
					s.HasLoans,
					s.HasLeases)));


		//Act
		_repo.Update(existingApplication);
		await _repo.UnitOfWork.SaveChangesAsync();
		_context.ChangeTracker.Clear();
		var updatedApplication = await _repo.GetByIdAsync(existingApplication.Id);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.True(updatedApplication.Schools.Any(x => x.Details.Urn == addedSchool.Details.Urn));
	}

	[Fact]
	public async Task RecordAlreadyExists_SchoolRemoved___RemovedSchoolPersisted()
	{
		//Arrange
		var existingApplications = await _repo.GetAllAsync();
		var existingApplication = existingApplications.FirstOrDefault();

		Assert.NotNull(existingApplication);

		var mutatedSchoolList = new List<ISchool>();

		existingApplication.Update(existingApplication.ApplicationType, existingApplication.ApplicationStatus,
			existingApplication.Contributors.ToDictionary(x => x.Id, x => x.Details),
			mutatedSchoolList.Select(s =>
				new UpdateSchoolParameter(s.Id,
					s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.Safeguarding,
					s.LocalAuthorityReorganisationDetails,
					s.LocalAuthorityClosurePlanDetails,
					s.DioceseName,
					s.DioceseFolderIdentifier,
					s.PartOfFederation,
					s.FoundationTrustOrBodyName,
					s.FoundationConsentFolderIdentifier,
					s.ExemptionEndDate,
					s.MainFeederSchools,
					s.ResolutionConsentFolderIdentifier,
					s.ProtectedCharacteristics,
					s.FurtherInformation,
					s.Details,
					new List<KeyValuePair<int, LoanDetails>>(s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)))),
					new List<KeyValuePair<int, LeaseDetails>>(s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets)))),
					s.HasLoans,
					s.HasLeases)));

		//Act
		_repo.Update(existingApplication);
		await _repo.UnitOfWork.SaveChangesAsync();
		_context.ChangeTracker.Clear();
		var updatedApplication = await _repo.GetByIdAsync(existingApplication.Id);

		//Assert
		Assert.NotNull(updatedApplication);
		Assert.False(updatedApplication.Schools.Any());
	}

	private static T PickRandomElement<T>(IEnumerable<T> enumerable)
	{
		Random random = new();
		var list = enumerable.ToList();
		return list.ElementAt(random.Next(1, list.Count));
	}
}
