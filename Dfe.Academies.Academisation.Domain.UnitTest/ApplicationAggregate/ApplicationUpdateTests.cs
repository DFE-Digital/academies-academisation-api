using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate;

public class ApplicationUpdateTests
{
	private readonly Faker _faker = new();
	private readonly Fixture _fixture = new();

	public ApplicationUpdateTests()
	{
		// default settings to construct valid SchoolDetails
		_fixture.Customize<SchoolDetails>(sd =>
			sd.With(s => s.ApproverContactEmail, _faker.Internet.Email())
				.With(s => s.ContactChairEmail, _faker.Internet.Email())
				.With(s => s.ContactHeadEmail, _faker.Internet.Email())
				.With(s => s.MainContactOtherEmail, _faker.Internet.Email()));

		_fixture.Customize<UpdateSchoolParameter>(composer =>
				composer
					.With(s => s.Id, 0));
		_fixture.Customize(new AutoMoqCustomization());
	}

	[Fact]
	public void ExistingIsSubmitted___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.Submitted, 1);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.Select(s=> new UpdateSchoolParameter(
				s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
				s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
				));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationStatus));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void StatusChanged___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			ApplicationStatus.Submitted,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
				s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationStatus));
		Assert.Equivalent(expected, subject);
	}

	[Theory]
	[MemberData(nameof(BuildRandomDifferentTypes))]
	public void TypeChanged___ValidationErrorReturned_NotMutated(ApplicationType updated, ApplicationType existing)
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, existing);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			updated,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
				s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationType));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void ContributorEmailChanged___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1);
		Application expected = Clone(subject);

		var contributors = subject.Contributors.ToDictionary(c => c.Id, c => c.Details);
		int randomContributorKey = PickRandomElement(contributors.Keys);
		contributors[randomContributorKey] = contributors[randomContributorKey] with
		{
			EmailAddress = _faker.Internet.Email()
		};

		int index = contributors.Keys.ToList().IndexOf(randomContributorKey);

		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			contributors,
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
				s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, $"{nameof(Contributor)}[{index}].{nameof(ContributorDetails.EmailAddress)}");
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewContributorNonZeroId___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1);
		Application expected = Clone(subject);

		var contributorsUpdated = subject.Contributors.ToDictionary(s => s.Id, s => s.Details);
		contributorsUpdated.Add(_fixture.Create<int>(), _fixture.Create<ContributorDetails>());

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			contributorsUpdated,
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
				s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.Contributors));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 0);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.TrustBenefitDetails,
			s.OfstedInspectionDetails,
			s.SafeguardingDetails,
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
			s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
		).ToList();

		schoolsUpdated.Add(new UpdateSchoolParameter(0, 
			"",
			"",
			"",
			"",
			"",
			"",
			"",
			true,
			"",
			"",
			DateTimeOffset.Now, 
			"",
			"",
			null,
			"",
			_fixture.Create<SchoolDetails>() with { ApproverContactEmail = "InvalidEmail" },
		new List<KeyValuePair<int, LoanDetails>>(),
		new List<KeyValuePair<int, LeaseDetails>>()
		));

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(SchoolDetails.ApproverContactEmail));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewSchoolNonZeroId___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 0);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.TrustBenefitDetails,
			s.OfstedInspectionDetails,
			s.SafeguardingDetails,
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
			s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
		).ToList();

		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>() with {Id = 99});

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.Schools));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewSchoolZeroUrn___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 0);
		Application expected = Clone(subject);

		var schoolsUpdated = new List<UpdateSchoolParameter>();
		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>() with { SchoolDetails = _fixture.Create<SchoolDetails>() with { Urn = 0 } });

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(SchoolDetails.Urn));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void UpdateExistingSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 3, ApplicationType.FormAMat);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.TrustBenefitDetails,
			s.OfstedInspectionDetails,
			s.SafeguardingDetails,
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
			s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
		).ToList();

		IEnumerable<int> allIndices = schoolsUpdated.Select((s, i) => new { Str = s, Index = i })
			.Select(x => x.Index);

		int randomKey = PickRandomElement(allIndices);
		schoolsUpdated[randomKey] = schoolsUpdated[randomKey] with 
									{ SchoolDetails = schoolsUpdated[randomKey].SchoolDetails 
										with { ContactHeadEmail = "ghjk" } };

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(SchoolDetails.ContactHeadEmail));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddMoreThanOneSchoolToSAP__ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.FormASat);
		Application expected = Clone(subject);

		var schoolsUpdated = new List<UpdateSchoolParameter>();

		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>());
		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>());

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(ApplicationType), "Cannot add more than one school when forming a single academy trust.");
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddMoreThanOneSchoolToJoinMAT__ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.JoinAMat);
		Application expected = Clone(subject);

		var schoolsUpdated = new List<UpdateSchoolParameter>();

		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>());
		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>());

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(ApplicationType), "Cannot add more than one school when joining a multi academy trust.");
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void UpdateExistingSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 3, ApplicationType.FormAMat);

		var updateSchoolParameters = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.TrustBenefitDetails,
			s.OfstedInspectionDetails,
			s.SafeguardingDetails,
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
			s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
		).ToList();

		IEnumerable<School> updateSchools = subject.Schools.Select(s => 
			new School(s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
						s.Loans.Select(l => new Loan(l.Id, l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)),
						s.Leases.Select(x => new Lease(x.Id, x.LeaseTerm, x.RepaymentAmount, x.InterestRate, x.PaymentsToDate, x.Purpose, x.ValueOfAssets, x.ResponsibleForAssets))));

		IEnumerable<int> allIndices = updateSchoolParameters.Select((s, i) => new { Str = s, Index = i })
			.Select(x => x.Index);

		int randomSchoolKey = PickRandomElement(allIndices);
		updateSchoolParameters[randomSchoolKey] = updateSchoolParameters[randomSchoolKey] with
		{
			SchoolDetails = updateSchoolParameters[randomSchoolKey].SchoolDetails
				with
			{ Urn = updateSchoolParameters[randomSchoolKey].SchoolDetails.Urn }
		};

		SchoolDetails updatedSchool = updateSchoolParameters[randomSchoolKey].SchoolDetails;
		int randomSchoolId = updateSchoolParameters[randomSchoolKey].Id;

		Application expected = new(
			subject.ApplicationId,
			subject.CreatedOn,
			subject.LastModifiedOn,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchools,
			subject.JoinTrust,
			subject.FormTrust);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchoolParameters);

		// assert
		DfeAssert.CommandSuccess(result);

		Assert.Equivalent(expected, subject);
		var schoolMutated = Assert.Single(subject.Schools, s => s.Id == randomSchoolId);
		Assert.Equivalent(updatedSchool, schoolMutated.Details);
	}

	[Fact]
	public void AddNewSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 0);

		var updateSchoolParameters = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.TrustBenefitDetails,
			s.OfstedInspectionDetails,
			s.SafeguardingDetails,
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
			s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id, new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose, l.ValueOfAssets, l.ResponsibleForAssets))).ToList())
		).ToList();

		var schoolDetailsToAdd = _fixture.Create<UpdateSchoolParameter>();
		updateSchoolParameters.Add(schoolDetailsToAdd);

		IEnumerable<School> updateSchools = subject.Schools.Select(s => 
			new School(s.Id, 
				s.TrustBenefitDetails,
				s.OfstedInspectionDetails,
				s.SafeguardingDetails,
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
				s.Loans.Select(l => new Loan(l.Id, l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)),
				s.Leases.Select(x => new Lease(x.Id, x.LeaseTerm, x.RepaymentAmount, x.InterestRate, x.PaymentsToDate, x.Purpose, x.ValueOfAssets, x.ResponsibleForAssets))));

		Application expected = new(
			subject.ApplicationId,
			subject.CreatedOn,
			subject.LastModifiedOn,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchools,
			subject.JoinTrust,
			subject.FormTrust);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchoolParameters);

		// assert
		DfeAssert.CommandSuccess(result);

		Assert.Equivalent(expected, subject);
		var addedSchool = Assert.Single(subject.Schools, s => s.Details.Urn == schoolDetailsToAdd.SchoolDetails.Urn);
		Assert.Equivalent(schoolDetailsToAdd.SchoolDetails, addedSchool.Details);
	}

	[Fact]
	public void SetJoinMatDetails___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 0, ApplicationType.JoinAMat);

		var updateJoinTrust = _fixture.Create<IJoinTrust>();

		Mock.Get(updateJoinTrust).Setup(x => x.Id).Returns(10101);
		Mock.Get(updateJoinTrust).Setup(x => x.UKPRN).Returns(295061);
		Mock.Get(updateJoinTrust).Setup(x => x.TrustName).Returns("Test Trust");
		Mock.Get(updateJoinTrust).Setup(x => x.ChangesToTrust).Returns(true);
		Mock.Get(updateJoinTrust).Setup(x => x.ChangesToTrustExplained).Returns("ChangesToTrustExplained, it has changed");
		Mock.Get(updateJoinTrust).Setup(x => x.ChangesToLaGovernance).Returns(true);
		Mock.Get(updateJoinTrust).Setup(x => x.ChangesToLaGovernanceExplained).Returns("ChangesToLaGovernanceExplained, it has changed");

		Application expected = new(
			subject.ApplicationId,
			subject.CreatedOn,
			subject.LastModifiedOn,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.Cast<School>().ToList(),
			updateJoinTrust,
			subject.FormTrust);

		// act
		var result = subject.SetJoinTrustDetails(
			updateJoinTrust.UKPRN,
			updateJoinTrust.TrustName,
			updateJoinTrust.ChangesToTrust,
			updateJoinTrust.ChangesToTrustExplained,
			updateJoinTrust.ChangesToLaGovernance,
			updateJoinTrust.ChangesToLaGovernanceExplained);

		// assert
		DfeAssert.CommandSuccess(result);

		Assert.Equal(updateJoinTrust.TrustName, subject.JoinTrust?.TrustName);
		Assert.Equal(updateJoinTrust.UKPRN, subject.JoinTrust?.UKPRN);
	}

	[Fact]
	public void SetJoinMatDetailsWrongApplicationType__ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.FormAMat);
		Application expected = Clone(subject);

		var updateJoinTrust = _fixture.Create<IJoinTrust>();

		// act
		var result = subject.SetJoinTrustDetails(
			updateJoinTrust.UKPRN,
			updateJoinTrust.TrustName,
			updateJoinTrust.ChangesToTrust,
			updateJoinTrust.ChangesToTrustExplained,
			updateJoinTrust.ChangesToLaGovernance,
			updateJoinTrust.ChangesToLaGovernanceExplained);

		// assert
		DfeAssert.CommandValidationError(result, nameof(ApplicationType));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void SetFormMatDetails___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.FormAMat);

		var updateJoinTrust = _fixture.Create<IFormTrust>();
		var formTrustDetails = _fixture.Create<FormTrustDetails>();

		Mock.Get(updateJoinTrust).Setup(x => x.TrustDetails).Returns(formTrustDetails);

		// act
		var result = subject.SetFormTrustDetails(formTrustDetails);

		// assert
		DfeAssert.CommandSuccess(result);
		Assert.Equivalent(subject.FormTrust!.TrustDetails, formTrustDetails);
	}

	[Fact]
	public void SetFormMatDetailsWrongApplicationType__ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.JoinAMat);
		Application expected = Clone(subject);

		var updateJoinTrust = _fixture.Create<IFormTrust>();
		var formTrustDetails = _fixture.Create<FormTrustDetails>();

		Mock.Get(updateJoinTrust).Setup(x => x.TrustDetails).Returns(formTrustDetails);

		// act
		var result = subject.SetFormTrustDetails(formTrustDetails);

		// assert
		DfeAssert.CommandValidationError(result, nameof(ApplicationType));
		Assert.Equivalent(expected, subject);
	}

	private Application BuildApplication(ApplicationStatus applicationStatus, int numberOfSchools, ApplicationType? type = null)
	{
		var schools = new List<School>();

		for (int i = 0; i < numberOfSchools; i++)
		{
			schools.Add(_fixture.Create<School>());
		}

		Application application = new(
			_fixture.Create<int>(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			type ?? _fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			schools,
			JoinTrust.Create(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.Create<bool>(), _fixture.Create<string>(), _fixture.Create<bool>(), _fixture.Create<string>()),
			null);

		return application;
	}

	public static IEnumerable<object[]> BuildRandomDifferentTypes()
	{
		var existing = new Fixture().Create<ApplicationType>();

		var otherTypes = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>().Where(t => t != existing);
		var updated = PickRandomElement(otherTypes);

		yield return new object[] { updated, existing };
	}

	private static T PickRandomElement<T>(IEnumerable<T> enumerable)
	{
		Random random = new();
		var list = enumerable.ToList();
		return list.ElementAt(random.Next(1, list.Count));
	}

	private static Application Clone(Application application)
	{
		return new(
			application.ApplicationId,
			application.CreatedOn,
			application.LastModifiedOn,
			application.ApplicationType,
			application.ApplicationStatus,
			application.Contributors.ToDictionary(c => c.Id, c => c.Details),
			application.Schools.Select(s => 
				new School(s.Id, s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.SafeguardingDetails,
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
					s.Details, s.Loans.Select(l => new Loan(l.Id, l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)),
					s.Leases.Select(x =>
						new Lease(x.Id, x.LeaseTerm, x.RepaymentAmount, x.InterestRate, x.PaymentsToDate, x.Purpose, x.ValueOfAssets, x.ResponsibleForAssets)))), application.JoinTrust, application.FormTrust
		);
	}
}

