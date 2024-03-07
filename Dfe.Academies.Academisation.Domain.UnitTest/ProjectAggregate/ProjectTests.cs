using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using FluentAssertions;
using Moq;
using System;
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
