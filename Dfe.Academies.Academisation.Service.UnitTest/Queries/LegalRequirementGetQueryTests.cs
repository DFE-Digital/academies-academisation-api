using AutoFixture;
using Dfe.Academies.Academisation.Domain.LegalRequirement;
using Dfe.Academies.Academisation.Domain.Core.LegalRequirements;
using Dfe.Academies.Academisation.IService.ServiceModels.LegalRequirement;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;
using Dfe.Academies.Academisation.IData.LegalRequirementAggregate;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries;

public class LegalRequirementGetQueryTests
{
	private readonly Fixture _fixture = new();
	private readonly Mock<ILegalRequirementGetDataQuery> _mockDataQuery = new();

	[Fact]
	public async Task WhenDataQueryReturnIsNotNull___ReturnsExpectedServiceModel()
	{
		//Arrange
		const int expectedId = 1;

		var details = _fixture.Create<LegalRequirementDetails>();
		LegalRequirement data = new(expectedId, default, default, details);

		_mockDataQuery.Setup(q => q.Execute(expectedId))
			.ReturnsAsync(data);

		LegalRequirementGetQuery query = new(_mockDataQuery.Object);

		//Act
		var result = await query.Execute(expectedId);

		//Assert
		Assert.Multiple(
			() => Assert.NotNull(result),
			() => Assert.IsType<LegalRequirementServiceModel>(result),
			() => Assert.Equal(expectedId, result!.Id)
		);
	}

	[Fact]
	public async Task WhenDataQueryReturnIsNull___ReturnsNull()
	{
		//Arrange
		const int requestedId = 4;

		LegalRequirementGetQuery query = new(_mockDataQuery.Object);

		//Act
		var result = await query.Execute(requestedId);

		//Assert
		Assert.Null(result);
	}
}
