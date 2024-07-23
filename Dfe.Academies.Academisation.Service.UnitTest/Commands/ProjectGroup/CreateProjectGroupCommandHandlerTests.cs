using System.ComponentModel.DataAnnotations;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ProjectGroup
{
	public class CreateProjectGroupCommandHandlerTests
	{
		private MockRepository _mockRepository;

		private Mock<IProjectGroupRepository> _mockProjectGroupRepository;

		private Mock<IDateTimeProvider> _mockDateTimeProvider;
		private CreateProjectGroupCommandValidator _validator;
		private Mock<IConversionProjectRepository> mockCnversionProjectRepository;
		private Mock<ILogger<CreateProjectGroupCommandHandler>> _mocklogger;

		public CreateProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);

			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_validator = new CreateProjectGroupCommandValidator();
			mockCnversionProjectRepository = _mockRepository.Create<IConversionProjectRepository>();
			_mocklogger  = new Mock<ILogger<CreateProjectGroupCommandHandler>>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}
		
		private CreateProjectGroupCommandHandler CreateProjectGroupCommandHandler()
		{
			return new CreateProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mockDateTimeProvider.Object,
				_validator,
				mockCnversionProjectRepository.Object,
				_mocklogger.Object);
		}

		[Fact]
		public async Task Handle_ValidCommandWithoutConversions_PersistsExpectedProjectGroup()
		{
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));

			// Arrange
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateTProjectProjectCommand();
			var cancellationToken = CancellationToken.None;

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReference
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once);
			mockCnversionProjectRepository.Verify(x => x.AreProjectsAssociateToAnotherProjectGroupAsync(It.IsAny<List<int>>(), It.Is<CancellationToken>(x => x == cancellationToken)), Times.Never());
			mockCnversionProjectRepository.Verify(x => x.UpdateProjectsWithProjectGroupIdAsync(It.IsAny<List<int>>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.Is<CancellationToken>(x => x == cancellationToken)), Times.Never());
		}

		[Fact]
		public async Task Handle_ValidCommandWithConversions_PersistsExpectedProjectGroup()
		{
			// Arrange
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = CreateValidCreateTProjectProjectCommand();
			var cancellationToken = CancellationToken.None;
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));
			mockCnversionProjectRepository.Setup(x => x.AreProjectsAssociateToAnotherProjectGroupAsync(It.Is<List<int>>(x => x == request.ConversionProjectsUrns), It.Is<CancellationToken>(x => x == cancellationToken))).ReturnsAsync(false); ;
			mockCnversionProjectRepository.Setup(x => x.UpdateProjectsWithProjectGroupIdAsync(It.Is<List<int>>(x => x[0] == request.ConversionProjectsUrns[0]), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
			
			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReference
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once);
			mockCnversionProjectRepository.Verify(x => x.AreProjectsAssociateToAnotherProjectGroupAsync(It.IsAny<List<int>>(), It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once());
			mockCnversionProjectRepository.Verify(x => x.UpdateProjectsWithProjectGroupIdAsync(It.Is<List<int>>(x => x == request.ConversionProjectsUrns), It.IsAny<int>(), It.IsAny<DateTime>(), It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once());
		}

		[Fact]
		public async Task Handle_InValidCommand_ReturnsBadRequest()
		{
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));

			// Arrange
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = new CreateProjectGroupCommand(string.Empty, [3424]);
			var cancellationToken = CancellationToken.None;

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			Assert.IsType<CommandValidationErrorResult>(result);
			(result! as CommandValidationErrorResult).ValidationErrors.Should().HaveCount(2);
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReference
			&& x.ReferenceNumber != null
			&& x.CreatedOn == now)), Times.Never());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Never());
		}

		private static CreateProjectGroupCommand CreateValidCreateTProjectProjectCommand(bool includeConversions = true)
		{
			string trustReference = "11112222";

			return new CreateProjectGroupCommand(trustReference, includeConversions ? [03823] : []);
		}
	}
}
