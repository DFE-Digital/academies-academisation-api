using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionProjectAggregate;

public class CreateTests
{
	private readonly Faker _faker = new();

	[Fact]
	public async Task ReturnsTypeConversionsProject()
	{
		AdvisoryBoardDecisionFactory target = new();
		var project = await target.Create(new Faker<AdvisoryBoardDecisionDetails>().create());

		Assert.IsType<AdvisoryBoardDecision>(project);
	}

    [Fact]
	public async Task WithId___ReturnsWithIdSet()
    {
		var expectedId = _faker.Random.Int(1, 1000);

		AdvisoryBoardDecisionFactory target = new();
		var project = await target.Create(expectedId);

		Assert.Equal(expectedId, project.Id);
    }
}