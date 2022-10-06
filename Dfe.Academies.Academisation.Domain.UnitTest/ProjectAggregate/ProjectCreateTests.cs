using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ProjectAggregate;

public class ProjectCreateTests
{
	private readonly Fixture _fixture = new Fixture();
	public ProjectCreateTests()
	{
		_fixture.Customize(new AutoMoqCustomization());
	}

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
			_fixture.Create<List<School>>(),
			_fixture.Create<IJoinTrust>(),
			null);

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
			new List<School> {_fixture.Create<School>()}, _fixture.Create<IJoinTrust>(),
			null);

		// Act
		var createResult = new ProjectFactory().Create(application);

		// Assert
		IProject project = Assert.IsType<CreateSuccessResult<IProject>>(createResult).Payload;
		var school = application.Schools.Single();		

		Assert.Multiple(			
			() => Assert.Equal(school.Details.Urn, project.Details.Urn),
			() => Assert.Equal("Converter Pre-AO (C)", project.Details.ProjectStatus),
			() => Assert.Equal(DateTime.Today.AddMonths(6), project.Details.OpeningDate),
			() => Assert.Equal("Converter", project.Details.AcademyTypeAndRoute),
			() => Assert.Equal(school.Details.ConversionTargetDate, project.Details.ProposedAcademyOpeningDate),
			() => Assert.Equal(25000, project.Details.ConversionSupportGrantAmount),
			() => Assert.Equal(school.Details.CapacityPublishedAdmissionsNumber.ToString(), project.Details.PublishedAdmissionNumber),
			() => Assert.Equal(ToYesNoString(school.Details.LandAndBuildings!.PartOfPfiScheme), project.Details.PartOfPfiScheme),
			() => Assert.Equal(ToYesNoString(school.Details.EqualitiesImpactAssessmentCompleted != EqualityImpact.NotConsidered), project.Details.EqualitiesImpactAssessmentConsidered),
			() => Assert.Equal(school.Details.ProjectedPupilNumbersYear1, project.Details.YearOneProjectedPupilNumbers),
			() => Assert.Equal(school.Details.ProjectedPupilNumbersYear2, project.Details.YearTwoProjectedPupilNumbers),
			() => Assert.Equal(school.Details.ProjectedPupilNumbersYear3, project.Details.YearThreeProjectedPupilNumbers)			
		);
	}

	private static string? ToYesNoString(bool? value)
	{
		if (!value.HasValue) return null;
		return value == true ? "Yes" : "No";
	}
}
