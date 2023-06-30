using Dfe.Academies.Academisation.WebApi.Controllers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller.Cypress
{
	public class CypressControllerTests
	{
		[Fact]
		public void CanConstructCypressController()
		{
			var mockMediatr = Mock.Of<IMediator>();
			var sut = new CypressDataController(mockMediatr);
			sut.Should().NotBeNull();
		}

	}
}
