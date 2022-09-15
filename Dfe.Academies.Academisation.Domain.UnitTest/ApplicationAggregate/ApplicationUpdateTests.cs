using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Bogus;
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
				.With(s => s.MainContactOtherEmail, _faker.Internet.Email()));

		_fixture.Customize<UpdateSchoolParameter>(composer =>
				composer
					.With(s => s.Id, 0));
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
			subject.Schools.Select(s=> new UpdateSchoolParameter(
				s.Id, 
				s.Details,
				s.Loans.Select(l => new KeyValuePair<int,LoanDetails>(l.Id, l.Details)).ToList())
				));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationStatus));
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
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.Details,
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
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
		Application subject = BuildApplication(ApplicationStatus.InProgress, existing);
		Application expected = Clone(subject);

		// act
		var result = subject.Update(
			updated,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.Details,
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.ApplicationType));
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
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.Details,
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, $"{nameof(Contributor)}[{index}].{nameof(ContributorDetails.EmailAddress)}");
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
			subject.Schools.Select(s => new UpdateSchoolParameter(
				s.Id, 
				s.Details,
				s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
			));

		// assert
		DfeAssert.CommandValidationError(result, nameof(Application.Contributors));
		Assert.Equivalent(expected, subject);
	}

	[Fact]
	public void AddNewSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.Details,
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
		).ToList();

		schoolsUpdated.Add(new UpdateSchoolParameter(0, 
			_fixture.Create<SchoolDetails>() with { ApproverContactEmail = "InvalidEmail" },
			new List<KeyValuePair<int, LoanDetails>>()
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
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.Details,
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
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
	public void UpdateExistingSchoolInvalidEmail___ValidationErrorReturned_NotMutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);
		Application expected = Clone(subject);

		var schoolsUpdated = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.Details,
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
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
	public void UpdateExistingSchool___SuccessReturned_Mutated()
	{
		// arrange
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var updateSchoolParameters = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.Details,
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
		).ToList();

		IEnumerable<School> updateSchools = subject.Schools.Select(s => 
			new School(s.Id, 
						s.Details,
						s.Loans.Select(l => new Loan(l.Id, l.Details))));

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
			updateSchools);

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
		Application subject = BuildApplication(ApplicationStatus.InProgress);

		var updateSchoolParameters = subject.Schools.Select(s => new UpdateSchoolParameter(
			s.Id, 
			s.Details,
			s.Loans.Select(l => new KeyValuePair<int, LoanDetails>(l.Id, l.Details)).ToList())
		).ToList();

		var schoolDetailsToAdd = _fixture.Create<UpdateSchoolParameter>();
		updateSchoolParameters.Add(schoolDetailsToAdd);

		IEnumerable<School> updateSchools = subject.Schools.Select(s => 
			new School(s.Id, 
				s.Details,
				s.Loans.Select(l => new Loan(l.Id, l.Details))));

		Application expected = new(
			subject.ApplicationId,
			subject.CreatedOn,
			subject.LastModifiedOn,
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			updateSchools);

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

	private Application BuildApplication(ApplicationStatus applicationStatus, ApplicationType? type = null)
	{
		Application application = new(
			_fixture.Create<int>(),
			DateTime.UtcNow,
			DateTime.UtcNow,
			type ?? _fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			_fixture.Create<List<School>>());

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
			application.Schools.Select(s => new School(s.Id,
																s.Details,
																s.Loans.Select(l => new Loan(l.Id, l.Details))))
		);
	}
}

