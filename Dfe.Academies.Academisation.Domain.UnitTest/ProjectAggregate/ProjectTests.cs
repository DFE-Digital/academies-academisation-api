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
    }
}
