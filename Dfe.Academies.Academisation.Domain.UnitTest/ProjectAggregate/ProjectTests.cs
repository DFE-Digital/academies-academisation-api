using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FluentAssertions;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ProjectAggregate
{
	public class ProjectTests
	{
		private MockRepository mockRepository;
		private Fixture _fixture;


		public ProjectTests()
		{
			this.mockRepository = new MockRepository(MockBehavior.Strict);

			_fixture = new Fixture();
		}

		private Project CreateProject(bool isFormAMat = false)
		{
			var projectDetails = _fixture.Build<ProjectDetails>().With(x => x.IsFormAMat, isFormAMat).Create();


			return new Project(101, projectDetails);
		}

		[Fact]
		public void SetExternalApplicationForm_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var project = this.CreateProject();
			bool ExternalApplicationFormSaved = false;
			var ExternalApplicationFormUrl = _fixture.Create<string>();

			// Act
			project.SetExternalApplicationForm(
				ExternalApplicationFormSaved,
				ExternalApplicationFormUrl);

			// Assert
			project.Details.ExternalApplicationFormSaved.Should().Be(ExternalApplicationFormSaved);
			project.Details.ExternalApplicationFormUrl.Should().Be(ExternalApplicationFormUrl);
			this.mockRepository.VerifyAll();
		}
		[Fact]
		public void SetRoute_ValidRoute_UpdatesRoute()
		{
			// Arrange
			var project = this.CreateProject();
			string route = "Converter";

			// Act
			project.SetRoute(route);

			// Assert
			project.Details.AcademyTypeAndRoute.Should().Be(route);
		}

		[Fact]
		public void SetFormAMatProjectId_ValidId_UpdatesIdForFormAMatRoute()
		{
			// Arrange
			var project = this.CreateProject(true);
			int id = _fixture.Create<int>();

			// Act
			project.SetFormAMatProjectId(id);

			// Assert
			project.FormAMatProjectId.Should().Be(id);
		}

		[Fact]
		public void SetFormAMatProjectId_InvalidRoute_DoesNotUpdateId()
		{
			// Arrange
			var project = this.CreateProject();
			project.SetRoute("Converter"); // Setting the route to something other than "Form a Mat"
			int id = _fixture.Create<int>();

			// Act
			project.SetFormAMatProjectId(id);

			// Assert
			project.FormAMatProjectId.Should().BeNull(); // Since the route is not "Form a Mat", the ID should not be set
		}




		[Fact]
		public void SetIncomingTrust_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var project = this.CreateProject();
			string trustReferrenceNumber = "Ref";
			string trustName = "Name";

			// Act
			project.SetIncomingTrust(trustReferrenceNumber, trustName);

			// Assert
			project.Details.TrustReferenceNumber.Should().Be(trustReferrenceNumber);
			project.Details.NameOfTrust.Should().Be(trustName);
			this.mockRepository.VerifyAll();
		}

		[Fact]
		public void AddSchoolImprovemmentPlan_WhenAddingOne_OnePlanIsAddedToCollection()
		{
			// Arrange
			var project = this.CreateProject();
			var arrangers = new List<SchoolImprovementPlanArranger>() { SchoolImprovementPlanArranger.LocalAuthority };
			var startDate = DateTime.UtcNow;
			// Act
			project.AddSchoolImprovementPlan(arrangers, null, "trust ceo", startDate, SchoolImprovementPlanExpectedEndDate.ToConversion, null, SchoolImprovementPlanConfidenceLevel.High, null);

			// Assert
			project.SchoolImprovementPlans.Count().Should().Be(1);
			var addedPlan = project.SchoolImprovementPlans.First();
			addedPlan.ArrangedBy.Should().BeEquivalentTo(arrangers);
			addedPlan.ArrangedByOther.Should().BeNull();
			addedPlan.ProvidedBy.Should().Be("trust ceo");
			addedPlan.StartDate.Should().Be(startDate);
			addedPlan.ExpectedEndDate.Should().Be(SchoolImprovementPlanExpectedEndDate.ToConversion);
			addedPlan.ExpectedEndDateOther.Should().BeNull();
			addedPlan.ConfidenceLevel.Should().Be(SchoolImprovementPlanConfidenceLevel.High);
			addedPlan.PlanComments.Should().BeNull();

			this.mockRepository.VerifyAll();
		}


		[Fact]
		public void UpdateSchoolImprovemmentPlan_WhenUpdatingOne_OnePlanIsUpdatedInCollection()
		{
			// Arrange
			var project = this.CreateProject();
			var arrangers = new List<SchoolImprovementPlanArranger>() { SchoolImprovementPlanArranger.LocalAuthority };
			var startDate = DateTime.UtcNow;
			// Act
			project.AddSchoolImprovementPlan(arrangers, null, "trust ceo", startDate, SchoolImprovementPlanExpectedEndDate.ToConversion, null, SchoolImprovementPlanConfidenceLevel.High, null);
			// could really do with been able to mock the collection out, this test is a little brittle
			project.UpdateSchoolImprovementPlan(0, arrangers, null, "head teacher", startDate.AddDays(1), SchoolImprovementPlanExpectedEndDate.ToAdvisoryBoard, null, SchoolImprovementPlanConfidenceLevel.Medium, "test comments");

			// Assert
			project.SchoolImprovementPlans.Count().Should().Be(1);
			var addedPlan = project.SchoolImprovementPlans.First();
			addedPlan.ArrangedBy.Should().BeEquivalentTo(arrangers);
			addedPlan.ArrangedByOther.Should().BeNull();
			addedPlan.ProvidedBy.Should().Be("head teacher");
			addedPlan.StartDate.Should().Be(startDate.AddDays(1));
			addedPlan.ExpectedEndDate.Should().Be(SchoolImprovementPlanExpectedEndDate.ToAdvisoryBoard);
			addedPlan.ExpectedEndDateOther.Should().BeNull();
			addedPlan.ConfidenceLevel.Should().Be(SchoolImprovementPlanConfidenceLevel.Medium);
			addedPlan.PlanComments.Should().Be("test comments");

			this.mockRepository.VerifyAll();
		}
	}
}
