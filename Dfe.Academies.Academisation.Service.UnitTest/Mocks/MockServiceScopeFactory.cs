using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Dfe.Academies.Academisation.Service.UnitTest.Mocks
{
	internal static class MockServiceScopeFactory
	{
		public static (Mock<IServiceScopeFactory>, Mock<T>) CreateMock<T>() where T : class
		{

			var mockScope = new Mock<IServiceScope>();
			var mockServiceProvider = new Mock<IServiceProvider>();
			var mockGenericModel = new Mock<T>();
			var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

			mockScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);
			mockServiceProvider.Setup(s => s.GetService(typeof(T))).Returns(mockGenericModel.Object);
			mockServiceScopeFactory.Setup(m => m.CreateScope()).Returns(mockScope.Object);

			return (mockServiceScopeFactory, mockGenericModel);
		}
	}
}
