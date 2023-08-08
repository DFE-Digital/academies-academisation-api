using System;
using System.Net.Http;
using Dfe.Academies.Academisation.Data.Http;
using Dfe.Academies.Academisation.IData.Http;
using Dfe.Academisation.CorrelationIdMiddleware;
using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.Http
{
	public class AcademiesApiClientFactoryFactoryTests
	{

		[Fact]
		public void AcademiesApiClientFactory_Should_Implement_IAcademiesApiClientFactory()
		{
			var sut = new AcademiesApiClientFactoryFactory(Mock.Of<IHttpClientFactory>());
			sut.Should().BeAssignableTo<IAcademiesApiClientFactory>();
		}

		[Fact]
		public void Create_Sets_CorrelationId_Header()
		{ 
			var sut = new AcademiesApiClientFactoryFactory(new MockHttpClientFactory(new MockHttpMessageHandler()));


			var correlation = new CorrelationContext();
			correlation.SetContext(Guid.NewGuid());
			
			var client = sut.Create(correlation);

			client.DefaultRequestHeaders.Should().ContainSingle(x => x.Key == "x-correlationId");
		}
	}
}
