using System;
using System.Collections.Generic;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Data.UnitTest.Contexts;

public class TestAdvisoryBoardDecisionContext : TestAcademisationContext
{
	private readonly Fixture _fixture = new();

	public TestAdvisoryBoardDecisionContext()
	{
		Seed();
	}

	protected override void SeedData()
	{
		using var context = CreateContext();
		context.Database.EnsureCreated();

		var timestamp = DateTime.UtcNow;

		var seed = new List<ConversionAdvisoryBoardDecision>
		{
			_fixture.Build<ConversionAdvisoryBoardDecision>()
				.With(d => d.Id, 1)
				.With(d => d.AdvisoryBoardDecisionDetails.ConversionProjectId, 1)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create(),
			_fixture.Build<ConversionAdvisoryBoardDecision>()
				.With(d => d.Id, 2)
				.With(d => d.AdvisoryBoardDecisionDetails.ConversionProjectId, 2)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create(),
			_fixture.Build<ConversionAdvisoryBoardDecision>()
				.With(d => d.Id, 3)
				.With(d => d.AdvisoryBoardDecisionDetails.ConversionProjectId, 3)
				.With(d => d.CreatedOn, timestamp)
				.With(d => d.LastModifiedOn, timestamp)
				.Create()
		};

		context.AddRange(seed);
		context.SaveChanges();
	}
}
