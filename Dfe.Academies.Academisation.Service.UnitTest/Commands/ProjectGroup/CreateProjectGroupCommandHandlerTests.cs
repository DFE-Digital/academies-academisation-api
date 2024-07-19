using System.ComponentModel.DataAnnotations;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ProjectGroupsAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup;
using FluentAssertions;
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

		public CreateProjectGroupCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);

			_mockProjectGroupRepository = _mockRepository.Create<IProjectGroupRepository>();
			_mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
			_validator = new CreateProjectGroupCommandValidator();

			var mockContext = new Mock<IUnitOfWork>();
			_mockProjectGroupRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
		}

		private CreateProjectGroupCommandHandler CreateProjectGroupCommandHandler()
		{
			return new CreateProjectGroupCommandHandler(
				_mockProjectGroupRepository.Object,
				_mockDateTimeProvider.Object,
				_validator);
		}

		[Fact]
		public async Task Handle_ValidCommand_PersistsExpectedProjectGroup()
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
			&& x.ReferenceNumber == request.ReferenceNumber
			&& x.CreatedOn == now)), Times.Once());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once);
		}

		[Fact]
		public async Task Handle_InValidCommand_ReturnsBadRequest()
		{
			var now = DateTime.Now;
			_mockDateTimeProvider.Setup(x => x.Now).Returns(now);
			_mockProjectGroupRepository.Setup(x => x.Insert(It.IsAny<Domain.ProjectGroupsAggregate.ProjectGroup>()));

			// Arrange
			var createTransferProjectCommandHandler = CreateProjectGroupCommandHandler();
			var request = new CreateProjectGroupCommand(string.Empty, "3424");
			var cancellationToken = CancellationToken.None;

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			Assert.IsType<CommandValidationErrorResult>(result);
			(result as CommandValidationErrorResult).ValidationErrors.Should().HaveCount(3);
			_mockProjectGroupRepository.Verify(x => x.Insert(It.Is<Domain.ProjectGroupsAggregate.ProjectGroup>(x => x.TrustReference == request.TrustReference
			&& x.ReferenceNumber == request.ReferenceNumber
			&& x.CreatedOn == now)), Times.Never());

			_mockProjectGroupRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Never());
		}

		private static CreateProjectGroupCommand CreateValidCreateTProjectProjectCommand()
		{
			string trustReference = "11112222";
			string referenceNumber = "97857253";

			return new CreateProjectGroupCommand(trustReference, referenceNumber);
		}
	}
}
