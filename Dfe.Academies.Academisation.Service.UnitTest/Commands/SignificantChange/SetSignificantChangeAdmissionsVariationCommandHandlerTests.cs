using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeAdmissionsVariationCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repository = new();
	private readonly SetSignificantChangeAdmissionVariationCommandHandler _handler;

	public SetSignificantChangeAdmissionsVariationCommandHandlerTests()
	{
		_handler = new SetSignificantChangeAdmissionVariationCommandHandler(
			_repository.Object,
			Mock.Of<ILogger<SetSignificantChangeAdmissionVariationCommandHandler>>());
	}

	[Fact]
	public async Task Handle_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var command = new SetSignificantChangeAdmissionVariationCommand(
			72, true, "", true, true, true, null, null, null);
		_repository.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repository.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
	}

	[Fact]
	public async Task Handle_WhenProjectExists_UpdatesAdmissionVariationAndPersistsChanges()
	{
		var project = CreateProject();
		var unitOfWork = new Mock<IUnitOfWork>();
		unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
		_repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
		_repository.Setup(x => x.GetSignificantChangeProjectById(2001212, It.IsAny<CancellationToken>())).ReturnsAsync(project);

		var result = await _handler.Handle(
			new SetSignificantChangeAdmissionVariationCommand(
				2001212,
				true,
				"www.example.com",
				false,
				true,
				false,
				"Local authority concerns",
				null,
				"Admissions team concerns"),
			CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.AdmissionsVariationRequired.Should().BeTrue();
		project.Details.AdmissionsVariationSupportingEvidence.Should().Be("www.example.com");
		project.Details.AdmissionsVariationLocalAuthoritySupportive.Should().BeFalse();
		project.Details.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation.Should().Be("Local authority concerns");
		project.Details.AdmissionsVariationPupilPlacePlanningSupportive.Should().BeTrue();
		project.Details.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation.Should().BeNull();
		project.Details.AdmissionsVariationAdmissionsTeamSupportive.Should().BeFalse();
		project.Details.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation.Should().Be("Admissions team concerns");
	
		_repository.Verify(x => x.Update(project), Times.Once);
		unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	private static SignificantChangeProject CreateProject() => SignificantChangeProject.Create(
		new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
		DateTime.UtcNow);
}
