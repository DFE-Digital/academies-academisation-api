using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Project;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.Legacy.Project;

public class LegacyProjectUpdateCommandTests
{
	private readonly Fixture _fixture = new();

	private readonly Mock<IProjectGetDataQuery> _getDataQueryMock = new();
	private readonly Mock<IProjectUpdateDataCommand> _updateApplicationCommandMock = new();
	private readonly LegacyProjectUpdateCommand _subject;

	public LegacyProjectUpdateCommandTests()
	{
		_subject = new LegacyProjectUpdateCommand(_getDataQueryMock.Object, _updateApplicationCommandMock.Object);
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
	public async Task UpdateValid___SuccessResultReturned()
	{
		// Arrange
		Mock<IProject> projectMock = new();
		var projectServiceModel = _fixture.Create<LegacyProjectServiceModel>();
		projectMock.Setup(x => x.UpdatePatch(It.IsAny<ProjectDetails>())).Returns(new CommandSuccessResult());
		_getDataQueryMock.Setup(x => x.Execute(projectServiceModel.Id))
			.ReturnsAsync(projectMock.Object);

		// Act
		var result = await _subject.Execute(projectServiceModel);

		// Assert
		Assert.Multiple(
			() => Assert.IsType<CommandSuccessResult>(result),
			() => _updateApplicationCommandMock.Verify(m => m.Execute(projectMock.Object), Times.Once)
		);
	}
}
