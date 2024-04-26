using System.Threading.Tasks;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateGetByProjectIdTests
{
	private readonly AdvisoryBoardDecisionGetDataByProjectIdQuery _target;

	public ConversionAdvisoryBoardDecisionStateGetByProjectIdTests()
	{
		var mockContext = new TestAdvisoryBoardDecisionContext().CreateContext();
		var repo = new AdvisoryBoardDecisionRepository(mockContext);
		_target = new(repo);
	}

	[Fact]
	public async Task WhenRecordExists___ShouldReturnExpectedConversionAdvisoryBoardDecision()
	{
		//Arrange
		const int expectedProjectId = 2;

		//Act
		var result = await _target.Execute(expectedProjectId);

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
		var result = await _target.Execute(id);

		//Assert
		Assert.Null(result);
	}
}
