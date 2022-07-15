using Dfe.Academies.Academisation.IDomain.ConversionApplicationAggregate;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest
{
	public class ApplicationCreateCommandTest
	{
		private static Mock<IConversionApplicationFactory> _conversionApplicationFactoryMock = new ();
		private static Mock<IContributorDetails> _contributerDetails = new ();

		//[Theory]
		//[InlineData(ApplicationType.FormAMat)]
		//[InlineData(ApplicationType.JoinAMat)]
		//public async void Create_CallsFactoryCreate_WithCorrectValues(ApplicationType applicationType)
		//{
		//	ApplicationCreateCommand createCommand = new(_conversionApplicationFactoryMock.Object);

		//	await createCommand.Create(applicationType, _contributerDetails.Object);

		//	_conversionApplicationFactoryMock
		//		.Verify(x =>	x.Create(It.Is<ApplicationType>(y => y == applicationType), It.Is<IContributorDetails>(y => y == _contributerDetails.Object)), Times.Once());
		//}

		public async void Create_CallsRepository_WithCorrectValues()
		{

		}
	}
}
