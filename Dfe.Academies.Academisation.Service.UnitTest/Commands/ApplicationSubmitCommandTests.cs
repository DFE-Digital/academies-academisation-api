using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands;

public class ApplicationSubmitCommandTests
{
	private readonly Fixture _fixture = new();

	private readonly Mock<IApplicationRepository> _applicationRepo = new();
	private readonly Mock<IConversionProjectRepository> _conversionRepo = new();
	private readonly Mock<IApplicationSubmissionService> _applicationSubmissionServiceMock = new();
	private readonly Mock<IApplication> _applicationMock = new();
	private readonly Mock<IProject> _projectMock = new();
	private readonly int _applicationId;

	private readonly ApplicationSubmitCommandHandler _subject;

	public ApplicationSubmitCommandTests()
	{
		_applicationId = _fixture.Create<int>();
		_subject = new ApplicationSubmitCommandHandler(
			_applicationRepo.Object,
			_conversionRepo.Object,
			_applicationSubmissionServiceMock.Object
		);
		_applicationRepo.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
		_conversionRepo.Setup(x => x.UnitOfWork).Returns(new Mock<IUnitOfWork>().Object);
	}

	[Fact]
	public async Task NotFound___NotPassedToDataLayer_NotFoundReturned()
	{
		// arrange
		_applicationRepo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync((Application?)null);

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<NotFoundCommandResult>(result);
		_applicationRepo.Verify(x => x.Update(It.IsAny<Application>()), Times.Never);
	}

	[Fact]
	public async Task SubmitApplicationValidationError___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_applicationRepo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);

		CommandValidationErrorResult commandValidationErrorResult = new(new List<ValidationError>());
		_applicationSubmissionServiceMock.Setup(x => x.SubmitApplication(_applicationMock.Object)).Returns(commandValidationErrorResult);

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CommandValidationErrorResult>(result);
		Assert.Equal(commandValidationErrorResult, result);
		_applicationRepo.Verify(x => x.Update(_applicationMock.Object), Times.Never);
	}

	[Fact]
	public async Task ProjectNotCreated___PassedToDataLayer_CommandSuccessReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_applicationRepo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);
		_applicationSubmissionServiceMock.Setup(x => x.SubmitApplication(_applicationMock.Object)).Returns(new CommandSuccessResult());
		//_conversionRepo.Setup(m => m.Execute(_projectMock.Object)).ReturnsAsync(_projectMock.Object);
		_applicationSubmissionServiceMock.Setup(m => m.SubmitApplication(_applicationMock.Object))
			.Returns(new CommandSuccessResult());

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CommandSuccessResult>(result);
		_conversionRepo.Verify(x => x.Insert(_projectMock.Object as Project), Times.Never);
		_applicationRepo.Verify(x => x.Update(_applicationMock.Object), Times.Once);		
	}
	
	[Fact]
	public async Task ProjectCreated___PassedToDataLayer_CreateSuccessReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_projectMock.SetupGet(p => p.Details).Returns(new ProjectDetails { Urn = 1 });
		_applicationRepo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);
		_applicationMock.Setup(x => x.Submit(It.IsAny<DateTime>())).Returns(new CommandSuccessResult());
		_applicationSubmissionServiceMock.Setup(m => m.SubmitApplication(_applicationMock.Object))
			.Returns(new CreateSuccessResult<IProject>(_projectMock.Object));

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CreateSuccessResult<ConversionProjectServiceModel>>(result);
		_conversionRepo.Verify(x => x.Insert(_projectMock.Object as Project), Times.Once);
		_applicationRepo.Verify(x => x.Update(_applicationMock.Object), Times.Once);		
	}

	[Fact]
	public async Task ProjectCreateValidationError___NotPassedToUpdateDataCommand_ValidationErrorsReturned()
	{
		// arrange
		_applicationMock.SetupGet(a => a.ApplicationType).Returns(ApplicationType.JoinAMat);
		_applicationRepo.Setup(x => x.GetByIdAsync(_applicationId)).ReturnsAsync(_applicationMock.Object);

		CreateValidationErrorResult createValidationErrorResult = new(new List<ValidationError>());

		_applicationSubmissionServiceMock.Setup(m => m.SubmitApplication(_applicationMock.Object))
			.Returns(createValidationErrorResult);

		// act
		var result = await _subject.Handle(new ApplicationSubmitCommand(_applicationId), default(CancellationToken));

		// assert
		Assert.IsType<CreateValidationErrorResult>(result);
		Assert.Equal(createValidationErrorResult, result);
		_conversionRepo.Verify(x => x.Insert(It.IsAny<Project>()), Times.Never);
		_applicationRepo.Verify(x => x.Update(_applicationMock.Object), Times.Never);
	}
}
