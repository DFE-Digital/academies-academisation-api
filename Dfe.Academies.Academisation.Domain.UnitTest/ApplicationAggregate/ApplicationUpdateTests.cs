using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using FluentAssertions;
using FluentAssertions.Equivalency;
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
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
					new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
					new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
						l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
				s.HasLoans, s.HasLeases)
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationStatus));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
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
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
					new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
					new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
						l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
				s.HasLoans,
				s.HasLeases)
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationStatus));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
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
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
					new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
					new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
						l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
				s.HasLoans,
				s.HasLeases)
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationType));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
	}

	[Fact]
	public void ContributorEmailChanged___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1);
		Application expected = Clone(subject);

		var contributors = subject.Contributors.ToDictionary(c => c.Id, c => c.Details);
		int randomContributorKey = PickRandomElement(contributors.Keys);

		contributors[randomContributorKey] = new ContributorDetails(contributors[randomContributorKey].FirstName,
			contributors[randomContributorKey].LastName, _faker.Internet.Email(),
			contributors[randomContributorKey].Role, contributors[randomContributorKey].OtherRoleName);

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
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
					new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
					new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
						l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
				s.HasLoans,
				s.HasLeases)
			));

		// assert
		DfeAssert.CommandValidationError(result,
			$"{nameof(Contributor)}[{index}].{nameof(ContributorDetails.EmailAddress)}");
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
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
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
					new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
				s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
					new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
						l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
				s.HasLoans,
				s.HasLeases)
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.Contributors));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
	}

	[Fact]
	public void AddNewSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 0);
		Application expected = Clone(subject);

		var sutBuilder = new SchoolDetailsBuilder();
		var schoolsUpdated = new List<UpdateSchoolParameter>();
		schoolsUpdated.Add(
			_fixture.Create<UpdateSchoolParameter>() with { SchoolDetails = sutBuilder.WithApproverContactEmail("badEmail").Build() });

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(SchoolDetails.ApproverContactEmail));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
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
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
				new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
				new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
					l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
			s.HasLoans,
			s.HasLeases)
		).ToList();

		schoolsUpdated.Add(_fixture.Create<UpdateSchoolParameter>() with { Id = 99 });

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
		var sutBuilder = new SchoolDetailsBuilder();

		var schoolsUpdated = new List<UpdateSchoolParameter>();
		schoolsUpdated.Add(
			_fixture.Create<UpdateSchoolParameter>() with { SchoolDetails = sutBuilder.WithUrn(0).Build() });

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(SchoolDetails.Urn));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
	}

	[Fact]
	public void UpdateExistingSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 3, ApplicationType.FormAMat);
		Application expected = Clone(subject);
		var sutBuilder = new SchoolDetailsBuilder();

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
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
				new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
				new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
					l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
			s.HasLoans,
			s.HasLeases)
		).ToList();

		IEnumerable<int> allIndices = schoolsUpdated.Select((s, i) => new { Str = s, Index = i })
			.Select(x => x.Index);

		int randomKey = PickRandomElement(allIndices);
		schoolsUpdated[randomKey] = new UpdateSchoolParameter(
			schoolsUpdated[randomKey].Id,
			schoolsUpdated[randomKey].TrustBenefitDetails,
			schoolsUpdated[randomKey].OfstedInspectionDetails,
			schoolsUpdated[randomKey].SafeguardingDetails,
			schoolsUpdated[randomKey].LocalAuthorityReorganisationDetails,
			schoolsUpdated[randomKey].LocalAuthorityClosurePlanDetails,
			schoolsUpdated[randomKey].DioceseName,
			schoolsUpdated[randomKey].DioceseFolderIdentifier,
			schoolsUpdated[randomKey].PartOfFederation,
			schoolsUpdated[randomKey].FoundationTrustOrBodyName,
			schoolsUpdated[randomKey].FoundationConsentFolderIdentifier,
			schoolsUpdated[randomKey].ExemptionEndDate,
			schoolsUpdated[randomKey].MainFeederSchools,
			schoolsUpdated[randomKey].ResolutionConsentFolderIdentifier,
			schoolsUpdated[randomKey].ProtectedCharacteristics,
			schoolsUpdated[randomKey].FurtherInformation,
			sutBuilder.WithDetails(schoolsUpdated[randomKey].SchoolDetails).WithContactHeadEmail("ghjk").Build(),
			schoolsUpdated[randomKey].Loans,
			schoolsUpdated[randomKey].Leases,
			schoolsUpdated[randomKey].HasLoans,
			schoolsUpdated[randomKey].HasLeases);


		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		DfeAssert.CommandValidationError(result, nameof(SchoolDetails.ContactHeadEmail));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
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
		DfeAssert.CommandValidationError(result, nameof(ApplicationType),
			"Cannot add more than one school when forming a single academy trust.");
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
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
		DfeAssert.CommandValidationError(result, nameof(ApplicationType),
			"Cannot add more than one school when joining a multi academy trust.");
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
	}

	[Fact]
	public void UpdateExistingSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 3, ApplicationType.FormAMat);
		Application original = Clone(subject);

		var sutBuilder = new SchoolDetailsBuilder();

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
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
				new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
				new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
					l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
			s.HasLoans,
			s.HasLeases)
		).ToList();

		IEnumerable<int> allIndices = updateSchoolParameters.Select((s, i) => new { Str = s, Index = i })
			.Select(x => x.Index);

		int randomSchoolKey = PickRandomElement(allIndices);
		updateSchoolParameters[randomSchoolKey] = new UpdateSchoolParameter(
			updateSchoolParameters[randomSchoolKey].Id,
			updateSchoolParameters[randomSchoolKey].TrustBenefitDetails,
			updateSchoolParameters[randomSchoolKey].OfstedInspectionDetails,
			updateSchoolParameters[randomSchoolKey].SafeguardingDetails,
			updateSchoolParameters[randomSchoolKey].LocalAuthorityReorganisationDetails,
			updateSchoolParameters[randomSchoolKey].LocalAuthorityClosurePlanDetails,
			updateSchoolParameters[randomSchoolKey].DioceseName,
			updateSchoolParameters[randomSchoolKey].DioceseFolderIdentifier,
			updateSchoolParameters[randomSchoolKey].PartOfFederation,
			updateSchoolParameters[randomSchoolKey].FoundationTrustOrBodyName,
			updateSchoolParameters[randomSchoolKey].FoundationConsentFolderIdentifier,
			updateSchoolParameters[randomSchoolKey].ExemptionEndDate,
			updateSchoolParameters[randomSchoolKey].MainFeederSchools,
			updateSchoolParameters[randomSchoolKey].ResolutionConsentFolderIdentifier,
			updateSchoolParameters[randomSchoolKey].ProtectedCharacteristics,
			updateSchoolParameters[randomSchoolKey].FurtherInformation,
			sutBuilder.WithDetails(updateSchoolParameters[randomSchoolKey].SchoolDetails).WithUrn(101).Build(),
			updateSchoolParameters[randomSchoolKey].Loans,
			updateSchoolParameters[randomSchoolKey].Leases,
			updateSchoolParameters[randomSchoolKey].HasLoans,
			updateSchoolParameters[randomSchoolKey].HasLeases);

		int randomSchoolId = updateSchoolParameters[randomSchoolKey].Id;

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchoolParameters);

		// assert
		DfeAssert.CommandSuccess(result);

		original.Should().NotBeEquivalentTo(subject, DefaultExcludes);
		var schoolMutated = Assert.Single(subject.Schools, s => s.Id == randomSchoolId);
		schoolMutated.Details.Urn.Should().Be(101);

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
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
				new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
				new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
					l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
			s.HasLoans,
			s.HasLeases)
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
				s.Leases.Select(x => new Lease(x.Id, x.LeaseTerm, x.RepaymentAmount, x.InterestRate, x.PaymentsToDate,
					x.Purpose, x.ValueOfAssets, x.ResponsibleForAssets)),
				s.HasLoans,
				s.HasLeases));

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

		var updateJoinTrust = _fixture.Create<JoinTrust>();

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
			updateJoinTrust.TrustReference,
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
			updateJoinTrust.TrustReference,
			updateJoinTrust.ChangesToTrust,
			updateJoinTrust.ChangesToTrustExplained,
			updateJoinTrust.ChangesToLaGovernance,
			updateJoinTrust.ChangesToLaGovernanceExplained);

		// assert
		DfeAssert.CommandValidationError(result, nameof(ApplicationType));
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
	}

	[Fact]
	public void FormMatDetailsAddTrustKeyPerson__SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.FormAMat);

		var updateJoinTrust = _fixture.Create<IFormTrust>();
		var formTrustDetails = _fixture.Create<FormTrustDetails>();

		Mock.Get(updateJoinTrust).Setup(x => x.TrustDetails).Returns(formTrustDetails);
		var dob = DateTime.Now.AddYears(-30);
		// act
		var result = subject.SetFormTrustDetails(formTrustDetails);
		result = subject.AddTrustKeyPerson("Bob Smith", dob, "test biography",
			new List<ITrustKeyPersonRole> { TrustKeyPersonRole.Create(KeyPersonRole.CEO, "1 year") });

		// assert
		DfeAssert.CommandSuccess(result);
		subject.FormTrust!.KeyPeople.Should().NotBeNullOrEmpty();
		subject.FormTrust!.KeyPeople.Should().HaveCount(1);
		subject.FormTrust!.KeyPeople.First().Name.Should().Be("Bob Smith");
		subject.FormTrust!.KeyPeople.First().DateOfBirth.Should().Be(dob);
		subject.FormTrust!.KeyPeople.First().Biography.Should().Be("test biography");
		subject.FormTrust!.KeyPeople.First().Roles.First().Role.Should().Be(KeyPersonRole.CEO);
		subject.FormTrust!.KeyPeople.First().Roles.First().TimeInRole.Should().Be("1 year");
	}

	[Fact]
	public void FormMatDetailsUpdateTrustKeyPerson__SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.FormAMat);

		var updateJoinTrust = _fixture.Create<IFormTrust>();
		var formTrustDetails = _fixture.Create<FormTrustDetails>();

		Mock.Get(updateJoinTrust).Setup(x => x.TrustDetails).Returns(formTrustDetails);
		var dob = DateTime.Now.AddYears(-30);

		var result = subject.SetFormTrustDetails(formTrustDetails);

		// act
		result = subject.AddTrustKeyPerson("Bob Smith", dob, "test biography",
			new List<ITrustKeyPersonRole> { TrustKeyPersonRole.Create(KeyPersonRole.CEO, "1 year") });

		result = subject.UpdateTrustKeyPerson(0, "Ted Glen", dob, "test biography",
			new List<ITrustKeyPersonRole> { TrustKeyPersonRole.Create(0, KeyPersonRole.Chair, "2 years") });

		// assert
		DfeAssert.CommandSuccess(result);
		subject.FormTrust!.KeyPeople.Should().NotBeNullOrEmpty();
		subject.FormTrust!.KeyPeople.Should().HaveCount(1);
		subject.FormTrust!.KeyPeople.First().Name.Should().Be("Ted Glen");
		subject.FormTrust!.KeyPeople.First().DateOfBirth.Should().Be(dob);
		subject.FormTrust!.KeyPeople.First().Biography.Should().Be("test biography");
		subject.FormTrust!.KeyPeople.First().Roles.First().Role.Should().Be(KeyPersonRole.Chair);
		subject.FormTrust!.KeyPeople.First().Roles.First().TimeInRole.Should().Be("2 years");
	}

	[Fact]
	public void FormMatDetailsDeleteTrustKeyPerson__SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, 1, ApplicationType.FormAMat);

		var updateJoinTrust = _fixture.Create<IFormTrust>();
		var formTrustDetails = _fixture.Create<FormTrustDetails>();

		Mock.Get(updateJoinTrust).Setup(x => x.TrustDetails).Returns(formTrustDetails);
		var dob = DateTime.Now.AddYears(-30);

		var result = subject.SetFormTrustDetails(formTrustDetails);
		var keyPerson = _fixture.Create<ITrustKeyPerson>();
		Mock.Get(keyPerson).Setup(x => x.Name).Returns("Bob Smith");
		Mock.Get(keyPerson).Setup(x => x.DateOfBirth).Returns(dob);
		Mock.Get(keyPerson).Setup(x => x.Biography).Returns("test biography");
		Mock.Get(keyPerson).Setup(x => x.Roles)
			.Returns(
				new List<ITrustKeyPersonRole> { TrustKeyPersonRole.Create(KeyPersonRole.CEO, "1 year") }.AsReadOnly);

		// act
		subject.AddTrustKeyPerson(keyPerson.Name, keyPerson.DateOfBirth, keyPerson.Biography, keyPerson.Roles);

		result = subject.DeleteTrustKeyPerson(0);

		// assert
		DfeAssert.CommandSuccess(result);
		subject.FormTrust!.KeyPeople.Should().NotBeNull();
		subject.FormTrust!.KeyPeople.Should().HaveCount(0);
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
		expected.Should().BeEquivalentTo(subject, DefaultExcludes);
	}

	[Fact]
	public void FormMatDeleteSchool__SuccessReturned_Mutated()
	{
		// arrange
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
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id,
				new LoanDetails(l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule))).ToList(),
			s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.Id,
				new LeaseDetails(l.LeaseTerm, l.RepaymentAmount, l.InterestRate, l.PaymentsToDate, l.Purpose,
					l.ValueOfAssets, l.ResponsibleForAssets))).ToList(),
			s.HasLoans,
			s.HasLeases)
		).ToList();

		var schoolDetailsToAdd = _fixture.Create<UpdateSchoolParameter>();
		updateSchoolParameters.Add(schoolDetailsToAdd);

		subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchoolParameters);

		// act
		var result = subject.DeleteSchool(schoolDetailsToAdd.SchoolDetails.Urn);

		// assert
		DfeAssert.CommandSuccess(result);
		subject.Schools.Should().NotBeNull();
		subject.Schools.Should().HaveCount(0);
	}

	private Application BuildApplication(ApplicationStatus applicationStatus, int numberOfSchools,
		ApplicationType? type = null)
	{
		var schools = new List<School>();

		for (int i = 0; i < numberOfSchools; i++)
		{
			var ms = _fixture.Create<School>();
			var schoolToAdd = new School(ms.Id, ms.TrustBenefitDetails, ms.OfstedInspectionDetails,
				ms.SafeguardingDetails,
				ms.LocalAuthorityReorganisationDetails, ms.LocalAuthorityClosurePlanDetails, ms.DioceseName,
				ms.DioceseFolderIdentifier, ms.PartOfFederation, ms.FoundationTrustOrBodyName,
				ms.FoundationConsentFolderIdentifier, ms.ExemptionEndDate, ms.MainFeederSchools,
				ms.ResolutionConsentFolderIdentifier, ms.ProtectedCharacteristics, ms.FurtherInformation,
				new SchoolDetails(ms.Details.Urn, ms.Details.ProposedNewSchoolName!, ms.Details.LandAndBuildings,
					ms.Details.PreviousFinancialYear, ms.Details.CurrentFinancialYear, ms.Details.NextFinancialYear,
					ms.Details.ContactHeadName, _faker.Internet.Email(), ms.Details.ContactHeadTel,
					ms.Details.ContactChairName, _faker.Internet.Email(), ms.Details.ContactChairTel,
					ms.Details.ContactRole, ms.Details.MainContactOtherName,
					_faker.Internet.Email(),
					ms.Details.MainContactOtherTelephone, ms.Details.MainContactOtherRole,
					ms.Details.ApproverContactName, _faker.Internet.Email(),
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
				ms.Loans, ms.Leases, ms.HasLoans, ms.HasLeases);
			schools.Add(schoolToAdd);
		}

		Application application = new(
			_fixture.Create<int>(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			type ?? _fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			schools,
			JoinTrust.Create(_fixture.Create<int>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<ChangesToTrust>(),
				_fixture.Create<string>(), _fixture.Create<bool>(), _fixture.Create<string>()),
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
					s.Details,
					s.Loans.Select(l => new Loan(l.Id, l.Amount, l.Purpose, l.Provider, l.InterestRate, l.Schedule)),
					s.Leases.Select(x =>
						new Lease(x.Id, x.LeaseTerm, x.RepaymentAmount, x.InterestRate, x.PaymentsToDate, x.Purpose,
							x.ValueOfAssets, x.ResponsibleForAssets)),
					s.HasLoans,
					s.HasLeases)), application.JoinTrust, application.FormTrust
		);
	}

	public static EquivalencyAssertionOptions<_> DefaultExcludes<_>(EquivalencyAssertionOptions<_> opt)
	{
		opt.Excluding(m => m.Name.Equals("CreatedOn"));
		opt.Excluding(m => m.Name.Equals("LastModifiedOn"));
		opt.Excluding(m => m.Name.Contains("Dynamics"));
		opt.Excluding(m => m.Name.Contains("EntityId"));
		return opt;
	}

	public class SchoolDetailsBuilder
	{
		private readonly Fixture _fixture = new();
		private readonly Faker _faker = new();
		private SchoolDetails _defaultSchoolDetails;
		private string _approverEmail;
		private int _urn;
		private string _chairEmail;
		private string _otherEmail;
		private string _contactHeadEmail;

		public SchoolDetailsBuilder()
		{
			_defaultSchoolDetails = _fixture.Create<SchoolDetails>();
			_urn = _defaultSchoolDetails.Urn;
			_approverEmail = _faker.Internet.Email();
			_contactHeadEmail = _faker.Internet.Email();
			_otherEmail = _faker.Internet.Email();
			_chairEmail = _faker.Internet.Email();
		}

		public SchoolDetails Build()
		{
			return new SchoolDetails(
				_urn, _defaultSchoolDetails.SchoolName, _defaultSchoolDetails.LandAndBuildings,
				_defaultSchoolDetails.PreviousFinancialYear, _defaultSchoolDetails.CurrentFinancialYear,
				_defaultSchoolDetails.NextFinancialYear, _defaultSchoolDetails.ContactHeadName,
				_contactHeadEmail,
				_defaultSchoolDetails.ContactHeadTel, _defaultSchoolDetails.ContactChairName,
				_chairEmail, _defaultSchoolDetails.ContactChairTel,
				_defaultSchoolDetails.ContactRole, _defaultSchoolDetails.MainContactOtherName,
				_otherEmail, _defaultSchoolDetails.MainContactOtherTelephone,
				_defaultSchoolDetails.MainContactOtherRole, _defaultSchoolDetails.ApproverContactName,
				_approverEmail, _defaultSchoolDetails.ConversionTargetDateSpecified,
				_defaultSchoolDetails.ConversionTargetDate, _defaultSchoolDetails.ConversionTargetDateExplained,
				_defaultSchoolDetails.ConversionChangeNamePlanned, _defaultSchoolDetails.ProposedNewSchoolName,
				_defaultSchoolDetails.ApplicationJoinTrustReason, _defaultSchoolDetails.ProjectedPupilNumbersYear1,
				_defaultSchoolDetails.ProjectedPupilNumbersYear2, _defaultSchoolDetails.ProjectedPupilNumbersYear3,
				_defaultSchoolDetails.CapacityAssumptions, _defaultSchoolDetails.CapacityPublishedAdmissionsNumber,
				_defaultSchoolDetails.SchoolSupportGrantFundsPaidTo,
				_defaultSchoolDetails.ConfirmPaySupportGrantToSchool,
				_defaultSchoolDetails.SchoolHasConsultedStakeholders,
				_defaultSchoolDetails.SchoolPlanToConsultStakeholders,
				_defaultSchoolDetails.FinanceOngoingInvestigations,
				_defaultSchoolDetails.FinancialInvestigationsExplain,
				_defaultSchoolDetails.FinancialInvestigationsTrustAware, _defaultSchoolDetails.DeclarationBodyAgree,
				_defaultSchoolDetails.DeclarationIAmTheChairOrHeadteacher,
				_defaultSchoolDetails.DeclarationSignedByName, _defaultSchoolDetails.SchoolConversionReasonsForJoining);

		}

		public SchoolDetailsBuilder WithEmail(string email)
		{
			_approverEmail = email;

			return this;
		}

		public SchoolDetailsBuilder WithUrn(int urn)
		{
			_urn = urn;

			return this;
		}

		public SchoolDetailsBuilder WithContactHeadEmail(string contactHeadEmail)
		{
			_contactHeadEmail = contactHeadEmail;

			return this;
		}

		public SchoolDetailsBuilder WithDetails(SchoolDetails schoolDetails)
		{
			_defaultSchoolDetails = schoolDetails;

			return this;
		}

		public SchoolDetailsBuilder WithApproverContactEmail(string email)
		{
			_approverEmail = email;

			return this;
		}
	}
}


