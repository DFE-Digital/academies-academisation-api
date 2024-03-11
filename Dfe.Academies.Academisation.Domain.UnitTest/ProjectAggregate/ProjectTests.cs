using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
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

		private Project CreateProject()
		{
			var projectDetails = _fixture.Build<ProjectDetails>().Create();

			return new Project(101, projectDetails);
		}

		[Fact]
		public void SetExternalApplicationForm_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var project = this.CreateProject();
			bool ExternalApplicationFormSaved = false;
			string ExternalApplicationFormUrl = null;

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
			var project = this.CreateProject();
			project.SetRoute("Form a Mat"); // Setting the route to "Form a Mat"
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
	}
}
