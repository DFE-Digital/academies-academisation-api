using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.Extensions.Logging; 
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ProjectGroup
{
	public class DeleteProjectGroupCommandHandlerTests
	{
		private readonly MockRepository _mockRepository;
		private readonly Mock<IProjectGroupRepository> _mockProjectGroupRepository; 
		private readonly Mock<ILogger<DeleteProjectGroupCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private readonly CancellationToken _cancellationToken;
		private readonly DeleteProjectGroupCommandHandler _deleteProjectGroupCommandHandler;

		public DeleteProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);
			_cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>(); 
			_mocklogger = new Mock<ILogger<DeleteProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_deleteProjectGroupCommandHandler = new DeleteProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mocklogger.Object);
		}

		[Fact]
		public async Task Handle_ProjectGroupDoesNotExists_ReturnsNotFoundCommandResult()
		{
			// Arrange 
			var request = new DeleteProjectGroupCommand("Reference");
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync((Domain.ProjectGroupsAggregate.ProjectGroup?)null);

			// Act
			var result = await _deleteProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var notFoundCommandResult = Assert.IsType<NotFoundCommandResult>(result);
			_mockProjectGroupRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()), Times.Never());
			_mockProjectGroupRepository.Verify(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken), Times.Once());
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
		} 

		[Fact]
		public async Task Handle_ValidRequestWithNoRemovedConversions_ReturnsSuccess()
		{
			// Arrange
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			var request = new DeleteProjectGroupCommand(expectedProjectGroup.ReferenceNumber!);
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockProjectGroupRepository.Setup(x => x.Delete(expectedProjectGroup));

			// Act
			var result = await _deleteProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockProjectGroupRepository.Verify(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.Delete(expectedProjectGroup), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Once);
		}
	}
}
