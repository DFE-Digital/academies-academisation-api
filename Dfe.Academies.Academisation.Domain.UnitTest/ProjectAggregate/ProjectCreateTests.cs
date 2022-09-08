using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ProjectAggregate;

public class ProjectCreateTests
{
	private readonly Fixture _fixture = new Fixture();

	[Theory]
	[InlineData(ApplicationType.FormAMat)]
	[InlineData(ApplicationType.FormASat)]
	public void Create_UnsupportedApplicationType___ReturnsValidationErrorResult(ApplicationType applicationType)
	{
		// Arrange
		var now = DateTime.Now;

		var application = new Application(1, now, now, applicationType,
			_fixture.Create<ApplicationStatus>(),
			_fixture.Create<Dictionary<int, ContributorDetails>>(),
			_fixture.Create<Dictionary<int, SchoolDetails>>());

		// Act
		var project = new ProjectFactory().Create(application);

		CreateValidationErrorResult<IProject>? result = null;

		// Assert
		Assert.Multiple(
			() => result = Assert.IsType<CreateValidationErrorResult<IProject>>(project),
			() => Assert.Equal("Only projects of type JoinAMat are supported", result!.ValidationErrors.Single().ErrorMessage),
			() => Assert.Equal("ApplicationStatus", result!.ValidationErrors.Single().PropertyName)
		);
	}

	[Fact]
	public void Create_JoinAMatApplicationType___ReturnsCreateSuccessResult()
	{
		// Arrange
		var now = DateTime.Now;

		var application = new Application(1, now, now, ApplicationType.JoinAMat,
			_fixture.Create<ApplicationStatus>(),
			new Dictionary<int, ContributorDetails> { { 1, _fixture.Create<ContributorDetails>() } },
			new Dictionary<int, SchoolDetails> { { 1, _fixture.Create<SchoolDetails>() } });

		// Act
		var project = new ProjectFactory().Create(application);

		CreateSuccessResult<IProject>? result = null;

		// Assert
		Assert.Multiple(
			() => result = Assert.IsType<CreateSuccessResult<IProject>>(project),
			() => Assert.Equal(application.Schools.Single().Details.Urn, result!.Payload.Details.Urn)
		);
	}
}
