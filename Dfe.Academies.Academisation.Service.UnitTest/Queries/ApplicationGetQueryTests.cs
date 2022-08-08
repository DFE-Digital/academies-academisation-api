using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries
{
	public class ApplicationGetQueryTests
	{
		private readonly Mock<IApplicationGetDataQuery> _mockDataQuery = new ();

		[Fact]
		public async Task WhenDataQueryReturnIsNull___ReturnsNull()
		{
			//Arrange
			const int requestedId = 4;

			ApplicationGetQuery query = new(_mockDataQuery.Object);

			//Act
			var result = await query.Execute(requestedId);

			//Assert
			Assert.Null(result);
		}
	}
}
