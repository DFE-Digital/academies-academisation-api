using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using Dfe.Academies.Academisation.Service.Commands.Application;
using Dfe.Academies.Academisation.Service.UnitTest.Helpers;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands
{
	public class ApplicationCreateCommandTest
	{
		private readonly Mock<IApplicationFactory> _applicationFactoryMock = new();
		private readonly Mock<IApplicationRepository> _repo = new();
		private readonly Mock<IMapper> _mockMapper = new();
		private Fixture _fixture = new();

		[Theory]
		[InlineData(ApplicationType.FormAMat)]
		[InlineData(ApplicationType.JoinAMat)]
		public async Task ApplicationReturnedFromFactory___ApplicationPassedToDataLayer_ServiceModelReturned(ApplicationType applicationType)
		{
			// arrange
			var applicationCreateRequestModel = new ApplicationCreateRequestModelBuilder()
				.WithApplicationType(applicationType)
				.Build();

			var mockContext = new Mock<IUnitOfWork>();
			_repo.Setup(x => x.UnitOfWork).Returns(mockContext.Object);


			var app = _fixture.Create<Application>();

			_applicationFactoryMock
				.Setup(x => x.Create(It.IsAny<ApplicationType>(), It.IsAny<ContributorDetails>()))
				.Returns(new CreateSuccessResult<Application>(app));

			ApplicationCreateCommand subject = new(_applicationFactoryMock.Object, _repo.Object, _mockMapper.Object);

			// act
			var result = await subject.Execute(applicationCreateRequestModel);

			// assert
			_repo.Verify(x => x.Insert(It.Is<Application>(y => y == app)), Times.Once());

			var successResult = result as CreateSuccessResult<ApplicationServiceModel>;
			Assert.IsType<ApplicationServiceModel>(successResult!.Payload);
		}

		[Theory]
		[InlineData(ApplicationType.FormAMat)]
		[InlineData(ApplicationType.JoinAMat)]
		public async Task ValidationErrorReturnedFromFactory___ApplicationNotPassedToDataLayer_ValidationErrorReturned(ApplicationType applicationType)
		{
			// arrange
			var applicationCreateRequestModel = new ApplicationCreateRequestModelBuilder()
				.WithApplicationType(applicationType)
				.Build();

			_applicationFactoryMock
				.Setup(x => x.Create(It.IsAny<ApplicationType>(), It.IsAny<ContributorDetails>()))
				.Returns(new CreateValidationErrorResult(new List<ValidationError>()));

			ApplicationCreateCommand subject = new(_applicationFactoryMock.Object, _repo.Object, _mockMapper.Object);

			// act
			var result = await subject.Execute(applicationCreateRequestModel);

			// assert
			_repo
				.Verify(x => x.Insert(It.IsAny<Application>()), Times.Never());

			Assert.IsType<CreateValidationErrorResult>(result);
		}
	}
}
