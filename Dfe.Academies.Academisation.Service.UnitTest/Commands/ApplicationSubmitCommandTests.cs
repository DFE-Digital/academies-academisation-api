using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class ApplicationSubmitCommandTests
{
	private readonly Fixture fixture = new();

	private readonly Mock<IApplicationGetDataQuery> _getDataQueryMock = new();
	private readonly Mock<IApplicationUpdateDataCommand> _updateDataCommandMock = new();
	private readonly Mock<IProjectCreateDataCommand> _projectCreateDataCommand = new();
	private readonly Mock<IProjectFactory> _projectFactoryMock = new();
	private readonly Mock<IApplication> _applicationMock = new();
	private readonly Mock<IProject> _projectMock = new();
	private readonly int _applicationId;

	private readonly ApplicationSubmitCommand _subject;

	public ApplicationSubmitCommandTests()
	{
		_applicationId = fixture.Create<int>();
		_subject = new(_getDataQueryMock.Object, _updateDataCommandMock.Object, _projectFactoryMock.Object,
			_projectCreateDataCommand.Object);
	}

	[Fact]
	public async Task SubmitSuccessful___PassedToDataLayer_SuccessReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_getDataQueryMock.Setup(x => x.Execute(_applicationId)).ReturnsAsync(_applicationMock.Object);
		_applicationMock.Setup(x => x.Submit()).Returns(new CommandSuccessResult());
		_projectCreateDataCommand.Setup(m => m.Execute(_projectMock.Object)).ReturnsAsync(_projectMock.Object);
		_projectFactoryMock.Setup(m => m.Create(_applicationMock.Object))
			.Returns(new CreateSuccessResult<IProject>(_projectMock.Object));

		// act
		var result = await _subject.Execute(_applicationId);

		// assert
		Assert.IsType<CommandSuccessResult>(result);
		_applicationMock.Verify(x => x.Submit(), Times.Once());
		_projectCreateDataCommand.Verify(x => x.Execute(_projectMock.Object), Times.Once);
		_updateDataCommandMock.Verify(x => x.Execute(_applicationMock.Object), Times.Once);		
	}

	[Fact]
	public async Task NotFound___NotPassedToDataLayer_NotFoundReturned()
	{
		// arrange
		_getDataQueryMock.Setup(x => x.Execute(_applicationId)).ReturnsAsync((IApplication?)null);

		// act
		var result = await _subject.Execute(_applicationId);

		// assert
		Assert.IsType<NotFoundCommandResult>(result);
		_updateDataCommandMock.Verify(x => x.Execute(It.IsAny<IApplication>()), Times.Never);
	}

	[Fact]
	public async Task SubmitUnsuccessful___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_getDataQueryMock.Setup(x => x.Execute(_applicationId)).ReturnsAsync(_applicationMock.Object);

		CommandValidationErrorResult commandValidationErrorResult = new(new List<ValidationError>());
		_applicationMock.Setup(x => x.Submit()).Returns(commandValidationErrorResult);

		// act
		var result = await _subject.Execute(_applicationId);

		// assert
		Assert.IsType<CommandValidationErrorResult>(result);
		Assert.Equal(commandValidationErrorResult, result);
		_applicationMock.Verify(x => x.Submit(), Times.Once());
		_updateDataCommandMock.Verify(x => x.Execute(_applicationMock.Object), Times.Never);
	}

	[Fact]
	public async Task ProjectCreateUnsuccessful___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_getDataQueryMock.Setup(x => x.Execute(_applicationId)).ReturnsAsync(_applicationMock.Object);

		CreateValidationErrorResult<IProject> createValidationErrorResult = new(new List<ValidationError>());
		_applicationMock.Setup(x => x.Submit()).Returns(new CommandSuccessResult());

		_projectFactoryMock.Setup(m => m.Create(_applicationMock.Object))
			.Returns(createValidationErrorResult);

		// act
		var result = await _subject.Execute(_applicationId);

		// assert
		var errors = Assert.IsType<CommandValidationErrorResult>(result).ValidationErrors;
		Assert.Equal(createValidationErrorResult.ValidationErrors, errors);
		_applicationMock.Verify(x => x.Submit(), Times.Once());
		_projectCreateDataCommand.Verify(x => x.Execute(It.IsAny<IProject>()), Times.Never);
		_updateDataCommandMock.Verify(x => x.Execute(_applicationMock.Object), Times.Never);
	}
}
