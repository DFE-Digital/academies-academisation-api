using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateGetByProjectIdTests
{
	private readonly AdvisoryBoardDecisionRepository _target;
	private readonly IMediator _mediator;
	public ConversionAdvisoryBoardDecisionStateGetByProjectIdTests()
	{
		var mockContext = new TestAdvisoryBoardDecisionContext(_mediator).CreateContext();
		_target = new AdvisoryBoardDecisionRepository(mockContext);
	}

	[Fact]
	public async Task WhenRecordExists___ShouldReturnExpectedConversionAdvisoryBoardDecision()
	{
		//Arrange
		const int expectedProjectId = 2;

		//Act
		var result = await _target.GetConversionProjectDecsion(expectedProjectId);

		//Assert
		Assert.Multiple(
			() => Assert.NotNull(result),
			() => Assert.IsType<ConversionAdvisoryBoardDecision>(result),
			() => Assert.Equal(expectedProjectId, result!.AdvisoryBoardDecisionDetails.ConversionProjectId),
			() => Assert.NotEqual(default, result!.Id),
			() => Assert.NotEqual(default, result!.CreatedOn),
			() => Assert.NotEqual(default, result!.LastModifiedOn)
		);
	}

	[Fact]
	public async Task WhenRecordDoesNotExist___ShouldReturnNull()
	{
		//Arrange
		const int id = 4;

		//Act
		var result = await _target.GetConversionProjectDecsion(id);

		//Assert
		Assert.Null(result);
	}
}
