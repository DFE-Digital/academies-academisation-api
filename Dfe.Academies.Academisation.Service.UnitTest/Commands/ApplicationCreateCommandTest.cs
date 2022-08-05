using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Commands;
using Dfe.Academies.Academisation.Service.UnitTest.Helpers;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands
{
	public class ApplicationCreateCommandTest
	{
		private readonly Mock<IApplicationFactory> _conversionApplicationFactoryMock = new();
		private readonly Mock<IApplicationCreateDataCommand> _applicationCreateDataCommandMock = new();

		[Theory]
		[InlineData(ApplicationType.FormAMat)]
		[InlineData(ApplicationType.JoinAMat)]
		public async void ApplicationReturnedFromFactory___ApplicationPassedToDataLayer_ServiceModelReturned(ApplicationType applicationType)
		{
			// arrange
			var applicationCreateRequestModel = new ApplicationCreateRequestModelBuilder()
				.WithApplicationType(applicationType)
				.Build();

			Mock<IApplication> conversionApplicationMock = new Mock<IApplication>();
			conversionApplicationMock.SetupGet(x => x.Contributors)
				.Returns(new List<IContributor>().AsReadOnly());
			conversionApplicationMock.SetupGet(x => x.Schools)
				.Returns(new List<ISchool>().AsReadOnly());

			_conversionApplicationFactoryMock
				.Setup(x => x.Create(It.IsAny<ApplicationType>(), It.IsAny<ContributorDetails>()))
				.Returns(new CreateSuccessResult<IApplication>(conversionApplicationMock.Object));

			ApplicationCreateCommand subject = new(_conversionApplicationFactoryMock.Object, _applicationCreateDataCommandMock.Object);

			// act
			var result = await subject.Execute(applicationCreateRequestModel);

			// assert
			_applicationCreateDataCommandMock
				.Verify(x => x.Execute(It.Is<IApplication>(y => y == conversionApplicationMock.Object)), Times.Once());

			var successResult = result as CreateSuccessResult<ApplicationServiceModel>;
			Assert.IsType<ApplicationServiceModel>(successResult!.Payload);
		}

		[Theory]
		[InlineData(ApplicationType.FormAMat)]
		[InlineData(ApplicationType.JoinAMat)]
		public async void ValidationErrorReturnedFromFactory___ApplicationNotPassedToDataLayer_ValidationErrorReturned(ApplicationType applicationType)
		{
			// arrange
			var applicationCreateRequestModel = new ApplicationCreateRequestModelBuilder()
				.WithApplicationType(applicationType)
				.Build();

			_conversionApplicationFactoryMock
				.Setup(x => x.Create(It.IsAny<ApplicationType>(), It.IsAny<ContributorDetails>()))
				.Returns(new CreateValidationErrorResult<IApplication>(new List<ValidationError>()));

			ApplicationCreateCommand subject = new(_conversionApplicationFactoryMock.Object, _applicationCreateDataCommandMock.Object);

			// act
			var result = await subject.Execute(applicationCreateRequestModel);

			// assert
			_applicationCreateDataCommandMock
				.Verify(x => x.Execute(It.IsAny<IApplication>()), Times.Never());

			Assert.IsType<CreateValidationErrorResult<ApplicationServiceModel>>(result);
		}
	}
}
