﻿using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionProjectAggregate;

public class CreateTests
{
	private readonly Faker _faker = new();

	[Fact]
	public async Task Test1()
	{
		ConversionProjectFactory target = new();
		var project = await target.Create(_faker.Random.Int(1, 1000));

		Assert.IsType<ConversionProject>(project);
	}
}