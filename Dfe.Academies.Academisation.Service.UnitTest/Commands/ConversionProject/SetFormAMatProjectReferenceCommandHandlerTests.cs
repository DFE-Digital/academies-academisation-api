using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

public class SetFormAMatProjectReferenceCommandHandlerTests
{
	[Fact]
	public async Task Handle_ProjectExists_UpdatesProject()
	{
		// Arrange
		var mockRepo = new Mock<IConversionProjectRepository>();
		var project = new Mock<Project>();
		project.Setup(p => p.Id).Returns(1); // Assuming Project is your domain entity and it's mockable
		mockRepo.Setup(r => r.GetConversionProject(It.IsAny<int>())).ReturnsAsync(project.Object);

		var handler = new SetFormAMatProjectReferenceCommandHandler(mockRepo.Object, new NullLogger<SetFormAMatProjectReferenceCommandHandler>());

		// Act
		var result = await handler.Handle(new SetFormAMatProjectReferenceCommand(1, 100), CancellationToken.None);

		// Assert
		Assert.IsType<CommandSuccessResult>(result);
		mockRepo.Verify(r => r.Update(It.IsAny<Project>()), Times.Once);
		mockRepo.Verify(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
	}

}
