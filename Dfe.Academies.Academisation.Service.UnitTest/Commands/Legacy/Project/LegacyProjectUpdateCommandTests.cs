using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project;

public class LegacyProjectUpdateCommandTests
{
	private readonly Fixture _fixture = new();
	private class UnhandledCommandResult : CommandResult { }

	private readonly Mock<IConversionProjectRepository> _getDataQueryMock = new();
	private readonly Mock<IProjectUpdateDataCommand> _updateProjectCommandMock = new();
	private readonly ConversionProjectUpdateCommandHandler _subject;

	public LegacyProjectUpdateCommandTests()
	{
		_subject = new ConversionProjectUpdateCommandHandler(_getDataQueryMock.Object, _updateProjectCommandMock.Object);
	}

	[Fact]
	public async Task NotFound___NotFoundResultReturned()
	{
		// Arrange
		var projectServiceModel = _fixture.Create<ConversionProjectServiceModel>();
		_getDataQueryMock.Setup(x => x.GetConversionProject(projectServiceModel.Id, default)).ReturnsAsync((IProject?)null);

		// Act
		var result = await _subject.Handle(new ConversionProjectUpdateCommand(projectServiceModel.Id, projectServiceModel), default);

		// Assert
		Assert.IsType<NotFoundCommandResult>(result);
	}

	[Fact]
	public async Task CommandValidationErrorResult___CommandValidationErrorResultReturned()
	{
		// Arrange
		var projectServiceModel = _fixture.Create<ConversionProjectServiceModel>();
		var validationErrorResult = _fixture.Create<CommandValidationErrorResult>();
		var project = new Mock<IProject>();
		project.SetupGet(m => m.Details).Returns(_fixture.Create<ProjectDetails>());
		project.Setup(m => m.Update(It.IsAny<ProjectDetails>())).Returns(validationErrorResult);

		_getDataQueryMock.Setup(x => x.GetConversionProject(projectServiceModel.Id, default)).ReturnsAsync(project.Object);		

		// Act
		var result = await _subject.Handle(new ConversionProjectUpdateCommand(projectServiceModel.Id, projectServiceModel), default);

		// Assert
		Assert.Equal(validationErrorResult, result);
	}


	[Fact]
	public async Task UnsupportedCommandResult___ThrowsNotImplementedException()
	{
		// Arrange
		var projectServiceModel = _fixture.Create<ConversionProjectServiceModel>();
		var validationErrorResult = _fixture.Create<UnhandledCommandResult>();
		var project = new Mock<IProject>();
		project.SetupGet(m => m.Details).Returns(_fixture.Create<ProjectDetails>());
		project.Setup(m => m.Update(It.IsAny<ProjectDetails>())).Returns(validationErrorResult);

		_getDataQueryMock.Setup(x => x.GetConversionProject(projectServiceModel.Id, default)).ReturnsAsync(project.Object);

		// Act & Assert
		await Assert.ThrowsAsync<NotImplementedException>(() => _subject.Handle(new ConversionProjectUpdateCommand(projectServiceModel.Id, projectServiceModel), default));		
	}

	[Fact]
	public async Task UpdateValid___SuccessResultReturned()
	{
		// Arrange
		Mock<IProject> projectMock = new();
		projectMock.Setup(x => x.Details).Returns(_fixture.Create<ProjectDetails>());
		var projectServiceModel = _fixture.Create<ConversionProjectServiceModel>();
		projectMock.Setup(x => x.Update(It.IsAny<ProjectDetails>())).Returns(new CommandSuccessResult());
		_getDataQueryMock.Setup(x => x.GetConversionProject(projectServiceModel.Id, default))
			.ReturnsAsync(projectMock.Object);

		// Act
		var result = await _subject.Handle(new ConversionProjectUpdateCommand(projectServiceModel.Id, projectServiceModel), default);

		// Assert
		Assert.Multiple(
			() => Assert.IsType<CommandSuccessResult>(result),
			() => _updateProjectCommandMock.Verify(m => m.Execute(projectMock.Object), Times.Once)
		);
	}
}
