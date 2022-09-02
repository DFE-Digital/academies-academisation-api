using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Bogus;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Test;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
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
			.With(s => s.MainContactOtherEmail, _faker.Internet.Email())
	);

	}
	[Fact]
	public void ExistingIsSubmitted___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.Submitted);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Equal(nameof(Application.ApplicationStatus), error.PropertyName);

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void StatusChanged___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			ApplicationStatus.Submitted,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Equal(nameof(Application.ApplicationStatus), error.PropertyName);

		Assert.Equivalent(expected, subject);
	}

	[Theory]
	[MemberData(nameof(BuildRandomDifferentTypes))]
	public void TypeChanged___ValidationErrorReturned_NotMutated(ApplicationType updated, ApplicationType existing)
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress, existing);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			updated,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Equal(nameof(Application.ApplicationType), error.PropertyName);

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void ContributorEmailChanged___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
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
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Equal(
			$"{nameof(Contributor)}[{index}].{nameof(ContributorDetails.EmailAddress)}",
			error.PropertyName);

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewContributorNonZeroId___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var contributorsUpdated = subject.Contributors.ToDictionary(s => s.Id, s => s.Details);
		contributorsUpdated.Add(_fixture.Create<int>(), _fixture.Create<ContributorDetails>());

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			contributorsUpdated,
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Contains(nameof(Application.Contributors), error.PropertyName);

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.ToDictionary(s => s.Id, s => s.Details);
		schoolsUpdated.Add(0, _fixture.Create<SchoolDetails>() with { ApproverContactEmail = "InvalidEmail" });

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

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewSchoolNonZeroId___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.ToDictionary(s => s.Id, s => s.Details);
		schoolsUpdated.Add(_fixture.Create<int>(), _fixture.Create<SchoolDetails>());

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			schoolsUpdated);

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Contains(nameof(Application.Schools), error.PropertyName);

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void UpdateExistingSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.ToDictionary(c => c.Id, c => c.Details);
		int randomKey = PickRandomElement(schoolsUpdated.Keys);
		schoolsUpdated[randomKey] = schoolsUpdated[randomKey] with { ContactHeadEmail = "ghjk" };

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

		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void UpdateExistingSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var schoolsUpdated = subject.Schools.ToDictionary(c => c.Id, c => c.Details);

		int randomSchoolKey = PickRandomElement(schoolsUpdated.Keys);
		SchoolDetails updatedSchool = _fixture.Create<SchoolDetails>() with { Urn = schoolsUpdated[randomSchoolKey].Urn };

		schoolsUpdated[randomSchoolKey] = updatedSchool;

		Application expected = new(
			subject.ApplicationId,
			subject.CreatedOn,
			subject.LastModifiedOn,
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
		DfeAssertions.AssertCommandSuccess(result);

		Assert.Equivalent(expected, subject);
		var schoolMutated = Assert.Single(subject.Schools, s => s.Id == randomSchoolKey);
		Assert.Equivalent(updatedSchool, schoolMutated.Details);
	}

	[Fact]
	public void AddNewSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var schoolsUpdated = subject.Schools.ToDictionary(s => s.Id, s => s.Details);
		var schoolDetailsToAdd = _fixture.Create<SchoolDetails>();
		schoolsUpdated.Add(0, schoolDetailsToAdd);

		Application expected = new(
			subject.ApplicationId,
			subject.CreatedOn,
			subject.LastModifiedOn,
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
		DfeAssertions.AssertCommandSuccess(result);

		Assert.Equivalent(expected, subject);
		var addedSchool = Assert.Single(subject.Schools, s => s.Details.Urn == schoolDetailsToAdd.Urn);
		Assert.Equivalent(schoolDetailsToAdd, addedSchool.Details);
	}

	private Application BuildApplication(ApplicationStatus applicationStatus, ApplicationType? type = null)
	{
		Application application = new(
			_fixture.Create<int>(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			type ?? _fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			_fixture.Create<Dictionary<int, SchoolDetails>>());

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
			application.Schools.ToDictionary(s => s.Id, s => s.Details)
		);
	}
}

