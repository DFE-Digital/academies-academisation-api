using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.FormAMatProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
	public class CreateFormAMatAndChildConversionCommandTests
	{
		private readonly Fixture _fixture = new Fixture();
		private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
		private readonly Mock<ILogger<CreateFormAMatAndChildConversionCommandHandler>> _loggerMock = new Mock<ILogger<CreateFormAMatAndChildConversionCommandHandler>>();
		private readonly Mock<IConversionProjectRepository> _conversionProjectRepositoryMock = new Mock<IConversionProjectRepository>();
		private readonly Mock<IFormAMatProjectRepository> _formAMatProjectRepositoryMock = new Mock<IFormAMatProjectRepository>();
		private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new Mock<IDateTimeProvider>();
		private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();


		[Fact]
		public async Task Handle_SuccessScenario_ReturnsSuccessResult()
		{
			// Arrange
			var handler = CreateHandler();
			var command = _fixture.Create<CreateFormAMatAndChildConversionCommand>();
			var formAMatProject = _fixture.Create<FormAMatProject>();
			var newProject = _fixture.Create<Project>();
			var newProjectResult = new CreateSuccessResult<IProject>(newProject);

			SetupDateTimeProviderMock();
			SetupFormAMatProjectRepositoryMockForInsert(formAMatProject);
			SetupFormAMatProjectRepositoryMockForUpdate(formAMatProject);
			SetupMapperMock(newProjectResult);
			SetupFormAMatProjectRepositoryMock();
			SetupConversionProjectRepositoryMock();


			// Act
			var result = await handler.Handle(command, CancellationToken.None);

			// Assert
			Assert.IsType<CommandSuccessResult>(result);

			_formAMatProjectRepositoryMock.Verify(x => x.Insert(It.IsAny<FormAMatProject>()), Times.Once);
			_formAMatProjectRepositoryMock.Verify(x => x.Update(It.IsAny<FormAMatProject>()), Times.Once);
			_conversionProjectRepositoryMock.Verify(x => x.Insert(It.IsAny<Project>()), Times.Once);
		}

		private void SetupDateTimeProviderMock()
		{
			_dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.UtcNow);
		}

		private void SetupFormAMatProjectRepositoryMockForInsert(FormAMatProject formAMatProject)
		{
			_formAMatProjectRepositoryMock.Setup(x => x.Insert(It.IsAny<FormAMatProject>()))
										  .Callback<FormAMatProject>(project => project.Id = formAMatProject.Id);
		}

		private void SetupFormAMatProjectRepositoryMockForUpdate(FormAMatProject formAMatProject)
		{
			_formAMatProjectRepositoryMock.Setup(x => x.Update(It.Is<FormAMatProject>(p => p.Id == formAMatProject.Id)))
										  .Verifiable();
		}

		private void SetupMapperMock(CreateSuccessResult<IProject> newProjectResult)
		{
			_mapperMock.Setup(x => x.Map<NewProject>(It.IsAny<NewProjectServiceModel>()))
					   .Returns(new NewProject(
						   new NewProjectSchool("Sample School", 12345, DateTime.UtcNow, false, "Local Authority", "Region"),
						   new NewProjectTrust("Trust Name", "Reference Number"),
						   "yes",
						   "yes"
					   ));
			_mapperMock.Setup(x => x.Map<CreateResult>(It.IsAny<NewProject>()))
					   .Returns(newProjectResult);
		}
		private void SetupFormAMatProjectRepositoryMock()
		{
			_formAMatProjectRepositoryMock.SetupGet(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);
		}

		private void SetupConversionProjectRepositoryMock()
		{
			_conversionProjectRepositoryMock.SetupGet(repo => repo.UnitOfWork).Returns(_unitOfWorkMock.Object);
		}


		private CreateFormAMatAndChildConversionCommandHandler CreateHandler()
		{
			return new CreateFormAMatAndChildConversionCommandHandler(
				_mapperMock.Object,
				_conversionProjectRepositoryMock.Object,
				_loggerMock.Object,
				_formAMatProjectRepositoryMock.Object,
				_dateTimeProviderMock.Object);
		}
	}
}

