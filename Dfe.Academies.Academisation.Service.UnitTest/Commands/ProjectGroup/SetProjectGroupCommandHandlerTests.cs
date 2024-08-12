using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ProjectGroup
{
	public class SetProjectGroupCommandHandlerTests
	{
		private MockRepository _mockRepository;
		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;
		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private Mock<IConversionProjectRepository> _mockConversionProjectRepository;
		private Mock<ITransferProjectRepository> _mockTransferProjectRepository;
		private Mock<ILogger<SetProjectGroupCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private CancellationToken _cancellationToken;
		private SetProjectGroupCommandHandler _setProjectGroupCommandHandler;

		public SetProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);
			_cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_mockConversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mockTransferProjectRepository = _mockRepository.Create<ITransferProjectRepository>();
			_mocklogger = new Mock<ILogger<SetProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_setProjectGroupCommandHandler = new SetProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mocklogger.Object,
				_mockConversionProjectRepository.Object,
				_mockTransferProjectRepository.Object);
		}

		[Fact]
		public async Task Handle_ProjectGroupDoesNotExists_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var request = new SetProjectGroupCommand([], []);
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync((Domain.ProjectGroupsAggregate.ProjectGroup?)null);

			// Act
			var result = await _setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var notFoundCommandResult = Assert.IsType<NotFoundCommandResult>(result);
			_mockProjectGroupRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()), Times.Never());
			_mockProjectGroupRepository.Verify(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken), Times.Once());
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
		}

		[Fact]
		public async Task Handle_ValidRequestWithoutConversionsAndTranfers_ReturnsSuccess()
		{
			// Arrange
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			var request = new SetProjectGroupCommand(_fixture.Create<List<int>>(), _fixture.Create<List<int>>()) 
			{
				GroupReferenceNumber = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken)).ReturnsAsync([]);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);
			_mockTransferProjectRepository.Setup(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken)).ReturnsAsync([]);
			_mockTransferProjectRepository.Setup(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);

			// Act
			var result = await _setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()), Times.Never);
			_mockProjectGroupRepository.Verify(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Never());
			_mockTransferProjectRepository.Verify(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Never());

		}

		[Fact]
		public async Task Handle_ValidRequestWithNoRemovedConversionsAndTransfers_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var expectedProjects = _fixture.Create<List<Domain.ProjectAggregate.Project>>();
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			var expectedTransferProjects = GetTransferProjects(expectedProjectGroup.TrustUkprn, 3, expectedProjectGroup.Id);
			var request = new SetProjectGroupCommand(expectedProjects.Select(x => x.Id).ToList(), expectedTransferProjects.Select(x => x.Id).ToList())
			{
				GroupReferenceNumber = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken)).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()));
			_mockTransferProjectRepository.Setup(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken)).ReturnsAsync(expectedTransferProjects);
			_mockTransferProjectRepository.Setup(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);
			_mockTransferProjectRepository.Setup(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));
			
			// Act
			var result = await _setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Exactly(2));
		}

		[Fact]
		public async Task Handle_ValidRequestWithNoRemovedConversionsAndNoTransfers_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var expectedProjects = _fixture.Create<List<Domain.ProjectAggregate.Project>>();
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			var request = new SetProjectGroupCommand(expectedProjects.Select(x => x.Id).ToList(), null)
			{
				GroupReferenceNumber = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken)).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()));

			// Act
			var result = await _setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken), Times.Never);
			_mockTransferProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Never);
			_mockConversionProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Exactly(1));
		}

		[Fact]
		public async Task Handle_ValidRequestWithOneRemovedConversionsAndTranfers_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			var expectedProjects = _fixture.Create<List<Domain.ProjectAggregate.Project>>();
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken)).ReturnsAsync(1);
			var expectedTransferProjects = GetTransferProjects(expectedProjectGroup.TrustUkprn, 3, expectedProjectGroup.Id);
			var request = new SetProjectGroupCommand(expectedProjects.Take(2).Select(x => x.Id).ToList(), expectedTransferProjects.Take(2).Select(x => x.Id).ToList())
			{
				GroupReferenceNumber = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken)).ReturnsAsync([]);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()));
			_mockTransferProjectRepository.Setup(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken)).ReturnsAsync(expectedTransferProjects);
			_mockTransferProjectRepository.Setup(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);
			_mockTransferProjectRepository.Setup(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			// Act
			var result = await _setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()), Times.Exactly(3));
			_mockTransferProjectRepository.Verify(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()), Times.Exactly(3));
			_mockTransferProjectRepository.Verify(x => x.GetTransferProjectsByIdsAsync(request.TransferProjectIds!, _cancellationToken), Times.Once);
			_mockTransferProjectRepository.Verify(x => x.GetProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Exactly(2));
		}

		private List<ITransferProject> GetTransferProjects(string incomingTrustUkprn, int count, int? projectGroupId)
		{
			var transferProjects = new List<ITransferProject>();
			for (int i = 0; i < count; i++)
			{
				var transferAcademy = new TransferringAcademy(incomingTrustUkprn, "in trust", _fixture.Create<string>()[..8], "region", "local authority");
				var transferringAcademies = new List<TransferringAcademy>() { transferAcademy };
				var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "out trust", transferringAcademies, false, DateTime.Now);
				transferProject.SetProjectGroupId(projectGroupId);
				transferProject.SetId(i + 1);
				transferProjects.Add(transferProject);
			}

			return transferProjects;
		}
	}
}
