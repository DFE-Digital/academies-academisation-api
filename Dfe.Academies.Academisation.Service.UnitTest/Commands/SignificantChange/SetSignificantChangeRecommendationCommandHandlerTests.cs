using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.SignificantChange;

public class SetSignificantChangeFundingCommandHandlerTests
{
	private readonly Mock<ISignificantChangeProjectRepository> _repository = new();
	private readonly SetSignificantChangeFundingCommandHandler _handler;

	public SetSignificantChangeFundingCommandHandlerTests()
	{
		_handler = new SetSignificantChangeFundingCommandHandler(
			_repository.Object,
			Mock.Of<ILogger<SetSignificantChangeFundingCommandHandler>>());
	}

	[Fact]
	public async Task Handle_WhenProjectDoesNotExist_ReturnsNotFound()
	{
		var command = new SetSignificantChangeFundingCommand(100, FundingAnswer.Yes, null, "Business case");
		_repository.Setup(x => x.GetSignificantChangeProjectById(command.Id, It.IsAny<CancellationToken>()))
			.ReturnsAsync((SignificantChangeProject?)null);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.Should().BeOfType<NotFoundCommandResult>();
		_repository.Verify(x => x.Update(It.IsAny<SignificantChangeProject>()), Times.Never);
	}

	[Fact]
	public async Task Handle_WhenProjectExists_UpdatesFundingAndPersistsChanges()
	{
		var project = CreateProject();
		var unitOfWork = new Mock<IUnitOfWork>();
		unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
		_repository.Setup(x => x.UnitOfWork).Returns(unitOfWork.Object);
		_repository.Setup(x => x.GetSignificantChangeProjectById(200, It.IsAny<CancellationToken>())).ReturnsAsync(project);

		var result = await _handler.Handle(
			new SetSignificantChangeFundingCommand(200, FundingAnswer.No, "Funding is unavailable", "Business case"),
			CancellationToken.None);

		result.Should().BeOfType<CommandSuccessResult>();
		project.Details.FundingAnswer.Should().Be(FundingAnswer.No);
		project.Details.FundingAdditionalInformation.Should().Be("Funding is unavailable");
		project.Details.FundingSupportingEvidence.Should().Be("Business case");
		_repository.Verify(x => x.Update(project), Times.Once);
		unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

	private static SignificantChangeProject CreateProject() => SignificantChangeProject.Create(
		new SignificantChangeProjectOptions(123456, 1, "Test Trust", "12345678", "Change of age range", "Test School"),
		DateTime.UtcNow);
}