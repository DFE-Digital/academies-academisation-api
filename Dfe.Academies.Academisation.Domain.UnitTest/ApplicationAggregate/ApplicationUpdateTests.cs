using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ApplicationAggregate;

public class ApplicationUpdateTests
{
	private readonly Fixture _fixture = new();

	[Fact]
	public void ApplicationSubmitted___ValidationErrorReturned()
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
		Assert.Single(validationErrorResult.ValidationErrors, v => v.PropertyName == "ApplicationStatus");
	}

	[Theory]
	[MemberData(nameof(BuildRandomDifferentTypes))]
	public void ApplicationTypeChanged___ValidationErrorReturned(ApplicationType updated, ApplicationType existing)
	{
		// arrange
		var subject = BuildApplication(ApplicationStatus.InProgress, existing);
		var expectedPropertyName = nameof(Application.ApplicationType);

		// act
		var result = subject.Update(
			updated,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		var validationErrorResult = Assert.IsAssignableFrom<CommandValidationErrorResult>(result);
		var error = Assert.Single(validationErrorResult.ValidationErrors);
		Assert.Equal(expectedPropertyName, error.PropertyName);
	}

	[Fact]
	public void ExistingInProgress_TypeUnchanged___SuccessReturned()
	{
		// arrange
		var subject = BuildApplication(ApplicationStatus.InProgress);

		// act
		var result = subject.Update(
			subject.ApplicationType,
			subject.ApplicationStatus,
			subject.Contributors.ToDictionary(c => c.Id, c => c.Details),
			subject.Schools.ToDictionary(s => s.Id, s => s.Details));

		// assert
		Assert.IsAssignableFrom<CommandSuccessResult>(result);
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

	private static IEnumerable<object[]> BuildRandomDifferentTypes()
	{
		var existing = new Fixture().Create<ApplicationType>();

		var types = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>().Where(t => t != existing);
		Random random = new();
		var updated = types.ElementAt(random.Next(1, types.Count()));

		yield return new object[] { updated, existing };
	}
}

