using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using System.Collections.Generic;
using System.Linq;
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

	[Fact]
	public void ApplicationInPrograss___SuccessReturned()
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

	private Application BuildApplication(ApplicationStatus applicationStatus)
	{
		Application application = new(
			_fixture.Create<int>(),
			_fixture.Create<ApplicationType>(),
			applicationStatus,
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			_fixture.Create<Dictionary<int, SchoolDetails>>());

		return application;
	}
}

