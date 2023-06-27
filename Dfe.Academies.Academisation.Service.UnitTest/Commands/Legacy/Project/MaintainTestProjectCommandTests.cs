using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Moq;
using Xunit;
using static Dfe.Academies.Academisation.Domain.ProjectAggregate.Project;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project
{
	public class MaintainTestProjectCommandTests
	{
		private readonly Fixture _fixture = new();
		private class UnhandledCommandResult : CommandResult { }

		private readonly Mock<ITestProjectGetDataQuery> _getDataQueryMock = new();
		private readonly Mock<ITestProjectCreateDataCommand> _createProjectCommandMock = new();
		private readonly Mock<ITestProjectDeleteDataQuery> _deleteProjectCommandMock = new();
		private readonly MaintainTestProjectCommand _subject;

		public MaintainTestProjectCommandTests()
		{
			_subject = new MaintainTestProjectCommand(_getDataQueryMock.Object, _createProjectCommandMock.Object, _deleteProjectCommandMock.Object);
		}
		[Fact]
		public async Task Test_project_generated_when_not_present_SuccessResultReturned()
		{
			// Arrange
			Mock<IProject> projectMock = new();
			var project = _fixture.Create<Domain.ProjectAggregate.Project>();

			_getDataQueryMock.Setup(x => x.Execute("Cypress Project"))
				.ReturnsAsync((IProject?)null);
			// Aggregate will be called to CreateTestProject() when it isn't found by 
			projectMock.Setup(x => x.CreateTestProject()).Returns(new CreateSuccessResult<IProject>(project));


			
			// Act
			var result = await _subject.Execute();

			// Assert
			Assert.IsType<CommandSuccessResult>(result);

		}
	}
}
