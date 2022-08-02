﻿using AutoFixture;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IData.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels;
using Dfe.Academies.Academisation.Service.Queries;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries;

public class ConversionAdvisoryBoardDecisionGetQueryTests
{
	private readonly Fixture _fixture = new();
	private readonly Mock<IAdvisoryBoardDecisionGetDataQuery> _mockDataQuery = new();

	[Fact]
	public async Task WhenDataQueryReturnIsNotNull___ReturnsExpectedServiceModel()
	{
		//Arrange
		const int expectedId = 1;

		var details = _fixture.Create<AdvisoryBoardDecisionDetails>();
		ConversionAdvisoryBoardDecision data = new(expectedId, details);
		
		_mockDataQuery.Setup(q => q.Execute(expectedId))
			.ReturnsAsync(data);
		
		ConversionAdvisoryBoardDecisionGetQuery query = new(_mockDataQuery.Object);
		
		//Act
		var result = await query.Execute(expectedId);
		
		//Assert
		Assert.Multiple(
			() => Assert.NotNull(result),
			() => Assert.IsType<ConversionAdvisoryBoardDecisionServiceModel>(result),
			() => Assert.Equal(expectedId, result!.AdvisoryBoardDecisionId)
		);
	}

	[Fact]
	public async Task WhenDataQueryReturnIsNull___ReturnsNull()
	{
		//Arrange
		const int requestedId = 4;
	
		ConversionAdvisoryBoardDecisionGetQuery query = new(_mockDataQuery.Object);

		//Act
		var result = await query.Execute(requestedId);
		
		//Assert
		Assert.Null(result);
	}
}
