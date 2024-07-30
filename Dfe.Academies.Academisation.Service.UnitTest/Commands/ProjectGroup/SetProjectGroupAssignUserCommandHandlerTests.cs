
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ProjectGroup
{
	public class SetProjectGroupAssignUserCommandHandlerTests
	{
		private MockRepository _mockRepository;

		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;

		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private Mock<ILogger<SetProjectGroupAssignUserCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private CancellationToken _cancellationToken;
		private SetProjectGroupAssignUserCommandHandler setProjectGroupAssignUserCommandHandler;

		public SetProjectGroupAssignUserCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);

			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_mockConversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mocklogger = new Mock<ILogger<SetProjectGroupAssignUserCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			setProjectGroupAssignUserCommandHandler = new SetProjectGroupAssignUserCommandHandler(
				_mockProjectGroupRepository.Object,
				_mocklogger.Object,
				_mockConversionProjectRepository.Object);
		}

		[Fact]
		public async Task Handle_ProjectGroupDoesNotExists_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var request = new SetProjectGroupAssignUserCommand(Guid.NewGuid(), "Full Name", "fullName@test.com")
			{
				GroupReferenceNumber = _fixture.Create<string>()
			};
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync((Domain.ProjectGroupsAggregate.ProjectGroup?)null);

			// Act
			var result = await setProjectGroupAssignUserCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var notFoundCommandResult = Assert.IsType<NotFoundCommandResult>(result);
			_mockProjectGroupRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()), Times.Never());
			_mockProjectGroupRepository.Verify(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken), Times.Once());
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
		}

		[Fact]
		public async Task Handle_ProjectGroupExists_ReturnsSuccessCommandResult()
		{
			// Arrange
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			var expectedProjects = _fixture.Create<List<Domain.ProjectAggregate.Project>>();
			var request = new SetProjectGroupAssignUserCommand(Guid.NewGuid(), "Full Name", "fullName@test.com")
			{
				GroupReferenceNumber = _fixture.Create<string>()
			};
			_mockProjectGroupRepository.Setup(x => x.Update(expectedProjectGroup));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync(expectedProjects);

			// Act
			var result = await setProjectGroupAssignUserCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockProjectGroupRepository.Verify(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()), Times.Once());
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Once());
		}

	}
}
