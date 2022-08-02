using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using EntityFrameworkCore.Testing.Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest;

public class ConversionAdvisoryBoardDecisionStateGetTests
{
	private readonly Fixture _fixture = new();

	private readonly AcademisationContext _mockContext;
	
	public ConversionAdvisoryBoardDecisionStateGetTests()
	{
		_mockContext = Create.MockedDbContextFor<AcademisationContext>();
		
		List<ConversionAdvisoryBoardDecisionState> data = new()
		{
			_fixture.Build<ConversionAdvisoryBoardDecisionState>().With(s => s.Id, 1).Create(),
			_fixture.Build<ConversionAdvisoryBoardDecisionState>().With(s => s.Id, 2).Create(),
			_fixture.Build<ConversionAdvisoryBoardDecisionState>().With(s => s.Id, 3).Create(),
		};
		
		_mockContext.ConversionAdvisoryBoardDecisions.AddRange(data);
		_mockContext.SaveChanges();
	}

	[Fact]
	public async Task WhenRecordExists___ShouldReturnExpectedConversionAdvisoryBoardDecision()
	{
		const int expectedId = 2;
		AdvisoryBoardDecisionGetDataQuery query = new(_mockContext);

		//Act
		var result = await query.Execute(expectedId);

		//Assert
		Assert.Multiple(
			() => Assert.NotNull(result),
			() => Assert.IsType<ConversionAdvisoryBoardDecision>(result),
			() => Assert.Equal(expectedId, result!.Id)
		);
	}

	[Fact]
	public async Task WhenRecordDoesNotExist___ShouldReturnNull()
	{
		const int id = 4;
		AdvisoryBoardDecisionGetDataQuery query = new(_mockContext);

		//Act
		var result = await query.Execute(id);

		//Assert
		Assert.Null(result);
	}
}
