using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Xunit;
using Xunit.Sdk;

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
			.With(s => s.MainContactOtherEmail, _faker.Internet.Email())
	);

	}
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

	[Fact]
	public void AddNewSchoolInvalidEmail___ValidationErrorReturned()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var schoolsUpdated = subject.Schools.ToDictionary(s => s.Id, s => s.Details);
		schoolsUpdated.Add(0, _fixture.Create<SchoolDetails>() with { ApproverContactEmail = "InvalidEmail"});

		Application expected = new(
			subject.ApplicationId,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated
			);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);

		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Contains(nameof(SchoolDetails.ApproverContactEmail), error.PropertyName);
	}

	[Fact]
	public void UpdateExistingSchoolInvalidEmail___ValidationErrorReturned()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var schoolsUpdated = subject.Schools.ToDictionary(c => c.Id, c => c.Details);
		var randomKey = PickRandomElement(schoolsUpdated.Keys);
		schoolsUpdated[randomKey] = schoolsUpdated[randomKey] with { ContactHeadEmail  = "ghjk" };

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
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);

		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Contains(nameof(SchoolDetails.ContactHeadEmail), error.PropertyName);
	}
	
	[Fact]
	public void UpdateExistingSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var schoolsUpdated = subject.Schools.ToDictionary(c => c.Id, c => c.Details);

		int randomSchoolKey = PickRandomElement(schoolsUpdated.Keys);
		SchoolDetails updatedSchool =
			SetSchoolUrn(_fixture.Create<SchoolDetails>(), schoolsUpdated[randomSchoolKey].Urn);
		
		schoolsUpdated[randomSchoolKey] = updatedSchool;

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
		AssertCommandSuccess(result);
		Assert.Equivalent(expected, subject);
		Assert.Equivalent(subject.Schools.Single(s => s.Id == randomSchoolKey).Details, updatedSchool);
		Assert.Single(subject.Schools, s => s.Id == randomSchoolKey && s.Details == updatedSchool);
	}

	[Fact]
	public void AddNewSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var schoolsUpdated = subject.Schools.ToDictionary(s => s.Id, s => s.Details);
		var newKey = schoolsUpdated.Keys.Max() + 1;
		schoolsUpdated.Add(newKey, _fixture.Create<SchoolDetails>());

		Application expected = new(
			subject.ApplicationId,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated
			);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c=> c.Details),
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
	
	private static SchoolDetails SetSchoolUrn(SchoolDetails schoolDetails, int urn)
	{
		return schoolDetails with {
			Urn = urn
		};
	}

	public static IEnumerable<object[]> BuildRandomDifferentTypes()
	{
		var existing = new Fixture().Create<ApplicationType>();

		var otherTypes = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>().Where(t => t != existing);
		var updated = PickRandomElement(otherTypes);

		yield return new object[] { updated, existing };
	}

	private static T PickRandomElement<T>(IEnumerable<T> list)
	{
		Random random = new();
		return list.ElementAt(random.Next(1, list.Count()));
	}

	private static void AssertCommandSuccess(CommandResult commandResult)
	{
		if (commandResult is CommandValidationErrorResult validationErrorResult)
		{
			throw new FailException("Validation Error:" + string.Join(";", validationErrorResult.ValidationErrors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
		}

		Assert.IsAssignableFrom<CommandSuccessResult>(commandResult);
	}
}

