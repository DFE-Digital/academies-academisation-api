using System.Threading;
using AutoFixture;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
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
		private SetProjectGroupCommandValidator _validator;
		private Mock<IConversionProjectRepository> _mockCnversionProjectRepository;
		private Mock<ILogger<SetProjectGroupCommandHandler>> _mocklogger;
		private readonly Fixture _fixture = new();
		private CancellationToken _cancellationToken;

		public SetProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);
			_cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_validator = new SetProjectGroupCommandValidator();
			_mockCnversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mocklogger = new Mock<ILogger<SetProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			mockContext.Setup(x => x.BeginTransactionAsync()).ReturnsAsync((new Mock<IDbContextTransaction>()).Object);
			mockContext.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);
			var mockExecuteStrategy = new Mock<IExecutionStrategy>();
			mockContext.Setup(x => x.CreateExecutionStrategy()).Returns(mockExecuteStrategy.Object);
		}
		private SetProjectGroupCommandHandler SetProjectGroupCommandHandler()
		{
			return new SetProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mockDateTimeProvider.Object,
				_validator,
				_mocklogger.Object,
				_mockCnversionProjectRepository.Object);
		}

		[Fact]
		public async Task Handle_InValidCommand_ReturnsCommandValidationErrorResult()
		{
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));

			// Arrange
			var setProjectGroupCommandHandler = SetProjectGroupCommandHandler();
			var request = new SetProjectGroupCommand(string.Empty, [3424]); 

			// Act
			var result = await setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var validationError = Assert.IsType<CommandValidationErrorResult>(result);
			validationError.ValidationErrors.Should().HaveCount(2);
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Never());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
		}

		[Fact]
		public async Task Handle_ProjectGroupDoesNotExists_ReturnsNotFoundCommandResult()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var request = new SetProjectGroupCommand(_fixture.Create<string>()[..8], [3424]);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.Urn, _cancellationToken)).ReturnsAsync((Domain.ProjectGroupsAggregate.ProjectGroup?)null);
			var setProjectGroupCommandHandler = SetProjectGroupCommandHandler();

			// Act
			var result = await setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var notFoundCommandResult = Assert.IsType<NotFoundCommandResult>(result);
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Never());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Never());
		}

		[Fact]
		public async Task Handle_ValidRequestWithoutConversions_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectGroup(_fixture.Create<string>()[..8], now);
			expectedProjectGroup.SetProjectReference(1);
			var request = new SetProjectGroupCommand(expectedProjectGroup.TrustReference, _fixture.Create<List<int>>()) 
			{
				Urn = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.Urn, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockCnversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([]);
			var setProjectGroupCommandHandler = SetProjectGroupCommandHandler();

			// Act
			var result = await setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockCnversionProjectRepository.Verify(x => x.UnitOfWork.CommitTransactionAsync(), Times.Never);
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.CommitTransactionAsync(), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.Update(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber == request.Urn
			&& x.LastModifiedOn == now)), Times.Once);

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Once);
		}

		[Fact]
		public async Task Handle_ValidRequestConversions_ReturnsSuccess()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var expectedProjectGroup = _fixture.Create<Domain.ProjectGroupsAggregate.ProjectGroup>();
			expectedProjectGroup.SetProjectGroup(_fixture.Create<string>()[..8], now);
			expectedProjectGroup.SetProjectReference(1);
			var request = new SetProjectGroupCommand(expectedProjectGroup.TrustReference, _fixture.Create<List<int>>())
			{
				Urn = expectedProjectGroup.ReferenceNumber!
			};
			_mockProjectGroupRepository.Setup(x => x.Update(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			_mockProjectGroupRepository.Setup(x => x.GetByReferenceNumberAsync(request.Urn, _cancellationToken)).ReturnsAsync(expectedProjectGroup);
			_mockCnversionProjectRepository.Setup(x => x.GetProjectsByProjectGroupAsync(new List<int> { expectedProjectGroup.Id }, _cancellationToken)).ReturnsAsync([]);
			var setProjectGroupCommandHandler = SetProjectGroupCommandHandler();

			// Act
			var result = await setProjectGroupCommandHandler.Handle(
				request,
				_cancellationToken);

			// Assert
			var commandSuccessResult = Assert.IsType<CommandSuccessResult>(result);
			_mockCnversionProjectRepository.Verify(x => x.Update(It.IsAny<Domain.ProjectAggregate.Project>()), Times.Never);
			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.CommitTransactionAsync(), Times.Once);
			_mockProjectGroupRepository.Verify(x => x.Update(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustUrn
			&& x.ReferenceNumber == request.Urn
			&& x.LastModifiedOn == now)), Times.Once);

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == _cancellationToken)), Times.Once);
		}
	}
}
