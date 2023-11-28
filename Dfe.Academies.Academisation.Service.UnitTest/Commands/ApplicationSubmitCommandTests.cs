using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class ApplicationSubmitCommandTests
{
	private readonly Fixture _fixture = new();

	private readonly Mock<IApplicationRepository> _repo = new();
	private readonly Mock<IProjectCreateDataCommand> _projectCreateDataCommand = new();
	private readonly Mock<IApplicationSubmissionService> _applicationSubmissionServiceMock = new();
	private readonly Mock<IApplication> _applicationMock = new();
	private readonly Mock<IProject> _projectMock = new();
	private readonly int _applicationId;

	private readonly ApplicationSubmitCommandHandler _subject;

	public ApplicationSubmitCommandTests()
	{
		_applicationId = _fixture.Create<int>();
		_subject = new ApplicationSubmitCommandHandler(
			_repo.Object,
			_projectCreateDataCommand.Object,
			_applicationSubmissionServiceMock.Object
		);
		_repo.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
	}

	[Fact]
	public async Task NotFound___NotPassedToDataLayer_NotFoundReturned()
	{
		// arrange
		_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync((Application?)null);

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<NotFoundCommandResult>(result);
		_repo.Verify(x => x.Update(It.IsAny<Application>()), Times.Never);
	}

	[Fact]
	public async Task SubmitApplicationValidationError___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);

		CommandValidationErrorResult commandValidationErrorResult = new(new List<ValidationError>());
		_applicationSubmissionServiceMock.Setup(x => x.SubmitApplication(_applicationMock.Object)).Returns(commandValidationErrorResult);

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CommandValidationErrorResult>(result);
		Assert.Equal(commandValidationErrorResult, result);
		_repo.Verify(x => x.Update(_applicationMock.Object), Times.Never);
	}

	[Fact]
	public async Task ProjectNotCreated___PassedToDataLayer_CommandSuccessReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);
		_applicationSubmissionServiceMock.Setup(x => x.SubmitApplication(_applicationMock.Object)).Returns(new CommandSuccessResult());
		_projectCreateDataCommand.Setup(m => m.Execute(_projectMock.Object)).ReturnsAsync(_projectMock.Object);
		_applicationSubmissionServiceMock.Setup(m => m.SubmitApplication(_applicationMock.Object))
			.Returns(new CommandSuccessResult());

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CommandSuccessResult>(result);
		_projectCreateDataCommand.Verify(x => x.Execute(_projectMock.Object), Times.Never);
		_repo.Verify(x => x.Update(_applicationMock.Object), Times.Once);		
	}
	
	[Fact]
	public async Task ProjectCreated___PassedToDataLayer_CreateSuccessReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_projectMock.SetupGet(p => p.Details).Returns(new ProjectDetails { Urn = 1 });
		_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);
		_applicationMock.Setup(x => x.Submit(It.IsAny<DateTime>())).Returns(new CommandSuccessResult());
		_projectCreateDataCommand.Setup(m => m.Execute(_projectMock.Object)).ReturnsAsync(_projectMock.Object);
		_applicationSubmissionServiceMock.Setup(m => m.SubmitApplication(_applicationMock.Object))
			.Returns(new CreateSuccessResult<IProject>(_projectMock.Object));

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CreateSuccessResult<LegacyProjectServiceModel>>(result);
		_projectCreateDataCommand.Verify(x => x.Execute(_projectMock.Object), Times.Once);
		_repo.Verify(x => x.Update(_applicationMock.Object), Times.Once);		
	}

	[Fact]
	public async Task ProjectCreateValidationError___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_repo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);

		CreateValidationErrorResult createValidationErrorResult = new(new List<ValidationError>());

		_applicationSubmissionServiceMock.Setup(m => m.SubmitApplication(_applicationMock.Object))
			.Returns(createValidationErrorResult);

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CreateValidationErrorResult>(result);
		Assert.Equal(createValidationErrorResult, result);
		_projectCreateDataCommand.Verify(x => x.Execute(It.IsAny<IProject>()), Times.Never);
		_repo.Verify(x => x.Update(_applicationMock.Object), Times.Never);
	}
}
