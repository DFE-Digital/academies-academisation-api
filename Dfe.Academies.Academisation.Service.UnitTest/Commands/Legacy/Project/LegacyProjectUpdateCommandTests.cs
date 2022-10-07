using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Legacy.Project;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project;

public class LegacyProjectUpdateCommandTests
{
	private readonly Fixture _fixture = new();
	private class UnhandledCommandResult : CommandResult { }

	private readonly Mock<IProjectGetDataQuery> _getDataQueryMock = new();
	private readonly Mock<IProjectUpdateDataCommand> _updateProjectCommandMock = new();
	private readonly LegacyProjectUpdateCommand _subject;

	public LegacyProjectUpdateCommandTests()
	{
		_subject = new LegacyProjectUpdateCommand(_getDataQueryMock.Object, _updateProjectCommandMock.Object);
	}

	[Fact]
	public async Task NotFound___NotFoundResultReturned()
	{
		// Arrange
		var projectServiceModel = _fixture.Create<LegacyProjectServiceModel>();
		_getDataQueryMock.Setup(x => x.Execute(projectServiceModel.Id)).ReturnsAsync((IProject?)null);

		// Act
		var result = await _subject.Execute(projectServiceModel);

		// Assert
		Assert.IsType<NotFoundCommandResult>(result);
	}

	[Fact]
	public async Task CommandValidationErrorResult___CommandValidationErrorResultReturned()
	{
		// Arrange
		var projectServiceModel = _fixture.Create<LegacyProjectServiceModel>();
		var validationErrorResult = _fixture.Create<CommandValidationErrorResult>();
		var project = new Mock<IProject>();
		project.SetupGet(m => m.Details).Returns(_fixture.Create<ProjectDetails>());
		project.Setup(m => m.Update(It.IsAny<ProjectDetails>())).Returns(validationErrorResult);

		_getDataQueryMock.Setup(x => x.Execute(projectServiceModel.Id)).ReturnsAsync(project.Object);		

		// Act
		var result = await _subject.Execute(projectServiceModel);

		// Assert
		Assert.Equal(validationErrorResult, result);
	}


	[Fact]
	public async Task UnsupportedCommandResult___ThrowsNotImplementedException()
	{
		// Arrange
		var projectServiceModel = _fixture.Create<LegacyProjectServiceModel>();
		var validationErrorResult = _fixture.Create<UnhandledCommandResult>();
		var project = new Mock<IProject>();
		project.SetupGet(m => m.Details).Returns(_fixture.Create<ProjectDetails>());
		project.Setup(m => m.Update(It.IsAny<ProjectDetails>())).Returns(validationErrorResult);

		_getDataQueryMock.Setup(x => x.Execute(projectServiceModel.Id)).ReturnsAsync(project.Object);

		// Act & Assert
		var result = await Assert.ThrowsAsync<NotImplementedException>(() => _subject.Execute(projectServiceModel));		
	}

	[Fact]
	public async Task UpdateValid___SuccessResultReturned()
	{
		// Arrange
		Mock<IProject> projectMock = new();
		var projectServiceModel = _fixture.Create<LegacyProjectServiceModel>();
		projectMock.Setup(x => x.Update(It.IsAny<ProjectDetails>())).Returns(new CommandSuccessResult());
		_getDataQueryMock.Setup(x => x.Execute(projectServiceModel.Id))
			.ReturnsAsync(projectMock.Object);

		// Act
		var result = await _subject.Execute(projectServiceModel);

		// Assert
		Assert.Multiple(
			() => Assert.IsType<CommandSuccessResult>(result),
			() => _updateProjectCommandMock.Verify(m => m.Execute(projectMock.Object), Times.Once)
		);
	}
}
