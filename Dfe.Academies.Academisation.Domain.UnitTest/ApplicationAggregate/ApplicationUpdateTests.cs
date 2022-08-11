using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate;

public class ApplicationUpdateTests
{
	private readonly Faker _faker = new();
	private readonly Fixture _fixture = new();

	[Fact]
	public void ExistingIsSubmitted___ValidationErrorReturned()
	{
		// arrange
		var subject = BuildApplication(ApplicationStatus.Submitted);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);

		var error = Assert.Single(validationErrorResult.ValidationErrors);
		var expectedPropertyName = nameof(Application.ApplicationStatus);
		Assert.Equal(expectedPropertyName, error.PropertyName);
	}

	[Fact]
	public void StatusChanged___ValidationErrorReturned()
	{
		// arrange
		var subject = BuildApplication(ApplicationStatus.InProgress);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			ApplicationStatus.Submitted,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);

		var error = Assert.Single(validationErrorResult.ValidationErrors);
		var expectedPropertyName = nameof(Application.ApplicationStatus);
		Assert.Equal(expectedPropertyName, error.PropertyName);
	}

	[Theory]
	[MemberData(nameof(BuildRandomDifferentTypes))]
	public void TypeChanged___ValidationErrorReturned(ApplicationType updated, ApplicationType existing)
	{
		// arrange
		var subject = BuildApplication(ApplicationStatus.InProgress, existing);

		// act
		var result = subject.Update(
			updated,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);

		var error = Assert.Single(validationErrorResult.ValidationErrors);
		var expectedPropertyName = nameof(Application.ApplicationType);
		Assert.Equal(expectedPropertyName, error.PropertyName);
	}

	[Fact]
	public void ContributorEmailChanged___ValidationErrorReturned()
	{
		// arrange
		var subject = BuildApplication(ApplicationStatus.InProgress);

		var contributors = subject.Contributors.ToDictionary(c => c.Id, c => c.Details);
		int randomContributorKey = PickRandomElement(contributors.Keys);
		contributors[randomContributorKey] = new ContributorDetails(
			contributors[randomContributorKey].FirstName,
			contributors[randomContributorKey].LastName,
			_faker.Internet.Email(),
			contributors[randomContributorKey].Role,
			contributors[randomContributorKey].OtherRoleName);

		int index = contributors.Keys.ToList().IndexOf(randomContributorKey);

		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			contributors,
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);

		var error = Assert.Single(validationErrorResult.ValidationErrors);
		var expectedPropertyName = $"{nameof(Contributor)}[{index}].{nameof(ContributorDetails.EmailAddress)}";
		Assert.Equal(expectedPropertyName, error.PropertyName);
	}

	[Theory]
	[MemberData(nameof(SchoolUpdateDetails))]
	public void ExistingInProgress_SchoolChanged___SuccessReturned_Mutated(SchoolDetails schoolUpdateDetails)
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var contributorsUpdated = subject.Contributors.ToDictionary(c => c.Id, c => c.Details);

		var schoolsUpdated = subject.Schools.ToDictionary(c => c.Id, c => c.Details);

		int randomSchoolKey = PickRandomElement(schoolsUpdated.Keys);
		BuildSchoolUpdate(schoolsUpdated[randomSchoolKey], schoolUpdateDetails);

		Application expected = new(
			subject.ApplicationId,
			subject.ApplicationType,
			subject.ApplicationStatus,
			contributorsUpdated,
			schoolsUpdated);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		Assert.IsAssignableFrom<CommandSuccessResult>(result);
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void ExistingInProgress_SchoolAdded___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var newSchool = _fixture.Create<SchoolDetails>();

		var schoolsUpdated = subject.Schools.ToDictionary(c => c.Id, c => c.Details);
		schoolsUpdated.Add(_fixture.Create<int>() , newSchool);
		
		Application expected = new(
			subject.ApplicationId,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		Assert.IsAssignableFrom<CommandSuccessResult>(result);
		Assert.Equivalent(expected, subject);
	}

	private Application BuildApplication(ApplicationStatus applicationStatus, ApplicationType? type = null)
	{
		Application application = new(
			_fixture.Create<int>(),
			type ?? _fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			_fixture.Create<Dictionary<int, SchoolDetails>>());

		return application;
	}
	private static SchoolDetails BuildSchoolUpdate(SchoolDetails details, SchoolDetails schoolUpdateDetails)
	{
		return details with
		{
			ApproverContactEmail = schoolUpdateDetails.ApproverContactEmail ?? details.ApproverContactEmail,
			ApproverContactName = schoolUpdateDetails.ApproverContactName ?? details.ApproverContactName,
			ContactChairEmail = schoolUpdateDetails.ContactChairEmail ?? details.ApproverContactEmail,
			ContactChairName = schoolUpdateDetails.ContactChairName ?? details.ContactChairName,
			ContactChairTel = schoolUpdateDetails.ContactChairTel ?? details.ContactChairTel,
			ContactHeadEmail = schoolUpdateDetails.ContactHeadEmail ?? details.ContactHeadEmail,
			ContactHeadName = schoolUpdateDetails.ContactHeadName ?? details.ContactHeadName,
			ContactHeadTel = schoolUpdateDetails.ContactHeadTel ?? details.ContactHeadTel,
			ContactRole = schoolUpdateDetails.ContactRole ?? details.ContactRole,
			MainContactOtherEmail = schoolUpdateDetails.MainContactOtherEmail ?? details.MainContactOtherEmail,
			MainContactOtherName = schoolUpdateDetails.MainContactOtherName ?? details.MainContactOtherName,
			MainContactOtherTelephone = schoolUpdateDetails.MainContactOtherTelephone ?? details.MainContactOtherTelephone,
			MainContactOtherRole = schoolUpdateDetails.MainContactOtherRole ?? details.MainContactOtherRole,
			ConversionTargetDate = schoolUpdateDetails.ConversionTargetDate ?? details.ConversionTargetDate,
			ConversionTargetDateExplained = schoolUpdateDetails.ConversionTargetDateExplained ?? details.ConversionTargetDateExplained,
			ProposedNewSchoolName = schoolUpdateDetails.ProposedNewSchoolName ?? details.ProposedNewSchoolName,
			ProjectedPupilNumbersYear1 = schoolUpdateDetails.ProjectedPupilNumbersYear1 ?? details.ProjectedPupilNumbersYear1,
			ProjectedPupilNumbersYear2 = schoolUpdateDetails.ProjectedPupilNumbersYear2 ?? details.ProjectedPupilNumbersYear2,
			ProjectedPupilNumbersYear3 = schoolUpdateDetails.ProjectedPupilNumbersYear3 ?? details.ProjectedPupilNumbersYear3,
			CapacityAssumptions = schoolUpdateDetails.CapacityAssumptions ?? details.CapacityAssumptions,
			CapacityPublishedAdmissionsNumber = schoolUpdateDetails.CapacityPublishedAdmissionsNumber ?? details.CapacityPublishedAdmissionsNumber,
			ApplicationJoinTrustReason = schoolUpdateDetails.ApplicationJoinTrustReason ?? details.ApplicationJoinTrustReason
		};
	}

	public static IEnumerable<object[]> BuildRandomDifferentTypes()
	{
		var existing = new Fixture().Create<ApplicationType>();

		var otherTypes = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>().Where(t => t != existing);
		var updated = PickRandomElement(otherTypes);

		yield return new object[] { updated, existing };
	}

	public static IEnumerable<object[]> SchoolUpdateDetails()
	{
		var update = new Fixture().Build<SchoolDetails>()
			.Create();
		yield return new object[] { update };
	}

	private static T PickRandomElement<T>(IEnumerable<T> list)
	{
		Random random = new();
		return list.ElementAt(random.Next(1, list.Count()));
	}
}

