using Bogus;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IData.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest
{
	public class ApplicationCreateCommandTest
	{
		private readonly Faker _faker = new Faker();
		private static Mock<IConversionApplicationFactory> _conversionApplicationFactoryMock = new ();
		private static Mock<IApplicationCreateDataCommand> _applicationCreateDataCommandMock = new ();

		[Theory]
		[InlineData(ApplicationType.FormAMat)]
		[InlineData(ApplicationType.JoinAMat)]
		public async void ApplicationReturnedFromFactory___ApplicationPassedToDataLayer(ApplicationType applicationType)
		{
			// arrange
			var applicationCreateRequestModel = new ApplicationCreateRequestModelBuilder()
				.WithApplicationType(applicationType)
				.Build();

			Mock<IConversionApplication> conversionApplicationMock = new Mock<IConversionApplication>();
			conversionApplicationMock.SetupGet(x => x.Contributors)
				.Returns(new List<IContributor>().AsReadOnly());

			_conversionApplicationFactoryMock
				.Setup(x => x.Create(It.IsAny<ApplicationType>(), It.IsAny<ContributorDetails>()))
				.ReturnsAsync(conversionApplicationMock.Object);

			ApplicationCreateCommand sut = new(_conversionApplicationFactoryMock.Object, _applicationCreateDataCommandMock.Object);

			// act
			var result = await sut.Execute(applicationCreateRequestModel);

			// assert
			_applicationCreateDataCommandMock
				.Verify(x => x.Execute(It.Is<IConversionApplication>(y => y == conversionApplicationMock.Object)), Times.Once());

			Assert.IsType<ApplicationServiceModel>(result);
		}
	}
}
