﻿using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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
			_mocklogger = new Mock<ILogger<SetProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);

			_setProjectGroupCommandHandler = new SetProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mocklogger.Object,
				_mockConversionProjectRepository.Object);
		}

		[Fact]
		public async Task Handle_ProjectGroupDoesNotExists_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var request = new SetProjectGroupCommand([]);
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
		public async Task Handle_ValidRequestWithoutConversions_ReturnsSuccess()
		{
			// Arrange
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			var request = new SetProjectGroupCommand(_fixture.Create<List<int>>()) 
			{
				GroupReferenceNumber = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken)).ReturnsAsync([]);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync([]);

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
		}

		[Fact]
		public async Task Handle_ValidRequestWithNoRemovedConversions_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var expectedProjects = _fixture.Create<List<Domain.ProjectAggregate.Project>>();
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			var request = new SetProjectGroupCommand(expectedProjects.Select(x => x.Id).ToList())
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
			_mockConversionProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Once);
		}

		[Fact]
		public async Task Handle_ValidRequestWithOneRemovedConversions_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			var expectedProjects = _fixture.Create<List<Domain.ProjectAggregate.Project>>();
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectReference(1);
			_mockConversionProjectRepository.Setup(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken)).ReturnsAsync(1);
			var request = new SetProjectGroupCommand(expectedProjects.Take(2).Select(x => x.Id).ToList())
			{
				GroupReferenceNumber = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.GroupReferenceNumber, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockConversionProjectRepository.Setup(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken)).ReturnsAsync([]);
			_mockConversionProjectRepository.Setup(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken)).ReturnsAsync(expectedProjects);
			_mockConversionProjectRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()));

			// Act
			var result = await _setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockConversionProjectRepository.Verify(x => x.GetProjectsByIdsAsync(request.ConversionProjectIds, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.GetConversionProjectsByProjectGroupIdAsync(expectedProjectGroup.Id, _cancellationToken), Times.Once);
			_mockConversionProjectRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()), Times.Exactly(expectedProjects.Count));
			_mockConversionProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(_cancellationToken), Times.Once);
		}
	}
}
