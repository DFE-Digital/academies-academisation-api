using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
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
			_fixture.Create<JoinTrust>(),
			null);

		// Act
		var project = new ProjectFactory().Create(application);

		CreateValidationErrorResult? result = null;

		// Assert
		Assert.Multiple(
			() => result = Assert.IsType<CreateValidationErrorResult>(project),
			() => Assert.Equal("Only projects of type JoinAMat are supported", result!.ValidationErrors.Single().ErrorMessage),
			() => Assert.Equal("ApplicationStatus", result!.ValidationErrors.Single().PropertyName)
		);
	}

	[Fact]
	public void Create_JoinAMatApplicationType_WithNulls___ReturnsCreateSuccessResult()
	{
		// Arrange
		var now = DateTime.Now;

		var currentYear = _fixture.Build<FinancialYear>()
							.With(fy => fy.CapitalCarryForwardStatus, (RevenueType?) null)
							.With(fy => fy.RevenueStatus, (RevenueType?) null)
							.With(fy => fy.CapitalCarryForward, (decimal?)null)
							.With(fy => fy.Revenue, (decimal?)null)
							.Create();
		var nextYear = _fixture.Build<FinancialYear>()
							.With(fy => fy.CapitalCarryForwardStatus, (RevenueType?) null)
							.With(fy => fy.RevenueStatus, (RevenueType?) null)
							.Create();

		var schoolDetails = _fixture.Build<SchoolDetails>()
								.With(sd => sd.CurrentFinancialYear, currentYear)
								.With(sd => sd.NextFinancialYear, nextYear)
								.Create();

		var school = _fixture.Build<School>()
						.With(s => s.Details, schoolDetails)
						.Create();

		var application = new Application(1, now, now, ApplicationType.JoinAMat,
			_fixture.Create<ApplicationStatus>(),
			new Dictionary<int, ContributorDetails> { { 1, _fixture.Create<ContributorDetails>() } },
			new List<School> { school }, _fixture.Create<JoinTrust>(),
			null);

		// Act
		var createResult = new ProjectFactory().Create(application);

		// Assert
		IProject project = Assert.IsType<CreateSuccessResult<IProject>>(createResult).Payload;

		Assert.Multiple(
			() => Assert.Null(project.Details.RevenueCarryForwardAtEndMarchCurrentYear),
			() => Assert.Null(project.Details.ProjectedRevenueBalanceAtEndMarchNextYear),
			() => Assert.Null(project.Details.CapitalCarryForwardAtEndMarchCurrentYear),
			() => Assert.Null(project.Details.CapitalCarryForwardAtEndMarchNextYear)
		);
	}

	[Fact]
	public void Create_JoinAMatApplicationType___ReturnsCreateSuccessResult()
	{
		// Arrange
		var now = DateTime.Now;

		var currentYear = _fixture.Build<FinancialYear>()
							.With(fy => fy.CapitalCarryForwardStatus, RevenueType.Deficit)
							.With(fy => fy.RevenueStatus, RevenueType.Deficit)
							.Create();
		var nextYear = _fixture.Build<FinancialYear>()
							.With(fy => fy.CapitalCarryForwardStatus, RevenueType.Surplus)
							.With(fy => fy.RevenueStatus, RevenueType.Surplus)
							.Create();

		var schoolDetails = _fixture.Build<SchoolDetails>()
								.With(sd => sd.CurrentFinancialYear, currentYear)
								.With(sd => sd.NextFinancialYear, nextYear)
								.Create();

		var school = _fixture.Build<School>()
						.With(s => s.Details, schoolDetails)
						.Create();

		var application = new Application(1, now, now, ApplicationType.JoinAMat,
			_fixture.Create<ApplicationStatus>(),
			new Dictionary<int, ContributorDetails> { { 1, _fixture.Create<ContributorDetails>() } },
			new List<School> { school }, _fixture.Create<JoinTrust>(),
			null);

		// Act
		var createResult = new ProjectFactory().Create(application);

		// Assert
		IProject project = Assert.IsType<CreateSuccessResult<IProject>>(createResult).Payload;

		Assert.Multiple(
			() => Assert.Equal(school.Details.Urn, project.Details.Urn),
			() => Assert.Equal(school.Details.SchoolName, project.Details.SchoolName),
			() => Assert.Equal($"A2B_{application.ApplicationId}", project.Details.ApplicationReferenceNumber),
			() => Assert.Equal("Converter Pre-AO (C)", project.Details.ProjectStatus),
			() => Assert.Equal(application.ApplicationSubmittedDate, project.Details.ApplicationReceivedDate),
			() => Assert.Equal(application.JoinTrust?.TrustReference, project.Details.TrustReferenceNumber),
			() => Assert.Equal(application.JoinTrust?.TrustName, project.Details.NameOfTrust),
			() => Assert.Equal("Converter", project.Details.AcademyTypeAndRoute),
			() => Assert.Null(project.Details.ProposedConversionDate),
			() => Assert.Equal(25000, project.Details.ConversionSupportGrantAmount),
			() => Assert.Equal(school.Details.CapacityPublishedAdmissionsNumber.ToString(), project.Details.PublishedAdmissionNumber),
			() => Assert.Equal(ToYesNoString(school.Details.LandAndBuildings!.PartOfPfiScheme), project.Details.PartOfPfiScheme),
			() => Assert.Equal("Yes", project.Details.FinancialDeficit),
			() => Assert.Equal(school.Details.SchoolConversionReasonsForJoining, project.Details.RationaleForTrust),
			() => Assert.Equal(school.Details.CurrentFinancialYear!.FinancialYearEndDate, project.Details.EndOfCurrentFinancialYear),
			() => Assert.Equal(school.Details.NextFinancialYear!.FinancialYearEndDate, project.Details.EndOfNextFinancialYear),
			() => Assert.Equal(school.Details.CurrentFinancialYear!.Revenue * -1.0M, project.Details.RevenueCarryForwardAtEndMarchCurrentYear),
			() => Assert.Equal(school.Details.NextFinancialYear!.Revenue, project.Details.ProjectedRevenueBalanceAtEndMarchNextYear),
			() => Assert.Equal(school.Details.CurrentFinancialYear!.CapitalCarryForward * -1.0M, project.Details.CapitalCarryForwardAtEndMarchCurrentYear),
			() => Assert.Equal(school.Details.NextFinancialYear!.CapitalCarryForward, project.Details.CapitalCarryForwardAtEndMarchNextYear),
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
