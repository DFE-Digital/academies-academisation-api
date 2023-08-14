using AutoMapper;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Xunit;
using AutoFixture;
using FluentAssertions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferProjectBenefitsCommandHandlerTests
	{
		private MockRepository _mockRepository;

		private Mock<ITransferProjectRepository> _mockTransferProjectRepository;
		private readonly Mock<ILogger<SetTransferProjectBenefitsCommandHandler>> _mockLogger;


		public SetTransferProjectBenefitsCommandHandlerTests()
		{
			_mockRepository = new MockRepository(MockBehavior.Strict);

			_mockTransferProjectRepository = _mockRepository.Create<ITransferProjectRepository>();

			var mockContext = new Mock<IUnitOfWork>();
			_mockTransferProjectRepository.Setup(x => x.UnitOfWork).Returns(mockContext.Object);
			_mockLogger = new Mock<ILogger<SetTransferProjectBenefitsCommandHandler>>();
		}

		private SetTransferProjectBenefitsCommandHandler CreateHandler()
		{
			return new SetTransferProjectBenefitsCommandHandler(
				this._mockTransferProjectRepository.Object, _mockLogger.Object);
		}

		[Fact]
		public async Task Handle_ValidCommand_PersistsExpectedTransferProject()
		{
			this._mockTransferProjectRepository.Setup(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));

			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "23456789", new List<string> { "34567890" }, DateTime.Now);
			// Mock GetById to use our Transfer Project from above
			_mockTransferProjectRepository.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(transferProject);

			// Arrange
			var createTransferProjectCommandHandler = this.CreateHandler();
			SetTransferProjectBenefitsCommand request = CreateCommand();
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			this._mockTransferProjectRepository.Verify(x => x.Update(It.Is<Domain.TransferProjectAggregate.TransferProject>(x =>
						request.AnyRisks == x.AnyRisks &&
			request.EqualitiesImpactAssessmentConsidered == x.EqualitiesImpactAssessmentConsidered &&
			x.IntendedTransferBenefits.All(itb => request.IntendedTransferBenefits.SelectedBenefits.Contains(itb.SelectedBenefit) &&
			request.IntendedTransferBenefits.OtherBenefitValue == x.OtherBenefitValue &&
			request.OtherFactorsToConsider.HighProfile.ShouldBeConsidered == x.HighProfileShouldBeConsidered &&
			request.OtherFactorsToConsider.HighProfile.FurtherSpecification == x.HighProfileFurtherSpecification &&
			request.OtherFactorsToConsider.ComplexLandAndBuilding.ShouldBeConsidered == x.ComplexLandAndBuildingShouldBeConsidered &&
			request.OtherFactorsToConsider.ComplexLandAndBuilding.FurtherSpecification == x.ComplexLandAndBuildingFurtherSpecification &&
			request.OtherFactorsToConsider.FinanceAndDebt.ShouldBeConsidered == x.FinanceAndDebtShouldBeConsidered &&
			request.OtherFactorsToConsider.FinanceAndDebt.FurtherSpecification == x.FinanceAndDebtFurtherSpecification &&
			request.OtherFactorsToConsider.OtherRisks.ShouldBeConsidered == x.OtherRisksShouldBeConsidered &&
			request.OtherFactorsToConsider.OtherRisks.FurtherSpecification == x.OtherRisksFurtherSpecification &&
			request.IsCompleted == x.BenefitsSectionIsCompleted)

			)), Times.Once());

			// called twice to generate urn from database generated field
			this._mockTransferProjectRepository.Verify(x => x.UnitOfWork.SaveChangesAsync(It.Is<CancellationToken>(x => x == cancellationToken)), Times.Once());
		}

		[Fact]
		public async Task Handle_ValidCommand_ReturnsCommandSuccessResponse()
		{
			this._mockTransferProjectRepository.Setup(x => x.Update(It.IsAny<Domain.TransferProjectAggregate.TransferProject>()));
			var transferProject = Domain.TransferProjectAggregate.TransferProject.Create("12345678", "23456789", new List<string> { "34567890" }, DateTime.Now);
			// Mock GetById to use our Transfer Project from above
			_mockTransferProjectRepository.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(transferProject);

			// Arrange
			var createTransferProjectCommandHandler = this.CreateHandler();
			SetTransferProjectBenefitsCommand request = CreateCommand();
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await createTransferProjectCommandHandler.Handle(
				request,
				cancellationToken);

			// Assert
			result.GetType().Should().Be(typeof(CommandSuccessResult));
		}

		[Fact]
		public async Task Handle_TransferProjectNotFound_ReturnsNotFoundCommandResult()
		{
			// Arrange
			SetTransferProjectBenefitsCommand request = CreateCommand();
			var createTransferProjectCommandHandler = this.CreateHandler();
			_mockTransferProjectRepository.Setup(x => x.GetById(It.IsAny<int>()))
										 .ReturnsAsync((Domain.TransferProjectAggregate.TransferProject)null);
			CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

			// Act
			var result = await createTransferProjectCommandHandler.Handle(request, cancellationToken);

			// Assert
			result.Should().BeOfType<NotFoundCommandResult>();
			_mockLogger.Verify(logger =>
				logger.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => true),
					It.IsAny<Exception>(),
					It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
				Times.Once);
		}

		private static SetTransferProjectBenefitsCommand CreateCommand()
		{
			Fixture fixture = new Fixture();

			var id = fixture.Create<int>();
			var anyRisks = fixture.Create<bool>();
			bool? equalitiesImpactAssessmentConsidered = fixture.Create<bool>();
			List<string> selectedBenefits = fixture.Create<List<string>>();
			string otherBenefitValue = fixture.Create<string>();
			bool highProfileShouldBeConsidered = fixture.Create<bool>();
			string highProfileFurtherSpecification = fixture.Create<string>();
			bool complexLandAndBuildingShouldBeConsidered = fixture.Create<bool>();
			string complexLandAndBuildingFurtherSpecification = fixture.Create<string>();
			bool financeAndDebtShouldBeConsidered = fixture.Create<bool>();
			string financeAndDebtFurtherSpecification = fixture.Create<string>();
			bool otherRisksShouldBeConsidered = fixture.Create<bool>();
			string otherRisksFurtherSpecification = fixture.Create<string>();
			bool isCompleted = fixture.Create<bool>();

			// Act
			return new SetTransferProjectBenefitsCommand
			{
				Id = id,
				AnyRisks = anyRisks,
				EqualitiesImpactAssessmentConsidered = equalitiesImpactAssessmentConsidered,
				IsCompleted = isCompleted,
				IntendedTransferBenefits = new IntendedTransferBenefitDto() { OtherBenefitValue = otherBenefitValue, SelectedBenefits = selectedBenefits },
				OtherFactorsToConsider = new OtherFactorsToConsiderDto()
				{
					ComplexLandAndBuilding = new BenefitConsideredFactorDto() { FurtherSpecification = complexLandAndBuildingFurtherSpecification, ShouldBeConsidered = complexLandAndBuildingShouldBeConsidered },
					FinanceAndDebt = new BenefitConsideredFactorDto() { FurtherSpecification = financeAndDebtFurtherSpecification, ShouldBeConsidered = financeAndDebtShouldBeConsidered },
					HighProfile = new BenefitConsideredFactorDto() { FurtherSpecification = highProfileFurtherSpecification, ShouldBeConsidered = highProfileShouldBeConsidered },
					OtherRisks = new BenefitConsideredFactorDto() { FurtherSpecification = otherRisksFurtherSpecification, ShouldBeConsidered = otherRisksShouldBeConsidered },
				}
			};
		}
	}
}
