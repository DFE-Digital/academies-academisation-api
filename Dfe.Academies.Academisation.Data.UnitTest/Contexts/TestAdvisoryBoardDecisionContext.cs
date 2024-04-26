using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

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
			new ConversionAdvisoryBoardDecision(1, _fixture.Build<AdvisoryBoardDecisionDetails>()
				.With(d => d.ConversionProjectId, 1).Create(), 
				Enumerable.Empty<AdvisoryBoardDeferredReasonDetails>(), 
				Enumerable.Empty<AdvisoryBoardDeclinedReasonDetails>(),
				Enumerable.Empty<AdvisoryBoardWithdrawnReasonDetails>(),
				timestamp,timestamp),
			new ConversionAdvisoryBoardDecision(2,_fixture.Build<AdvisoryBoardDecisionDetails>()
				.With(d => d.ConversionProjectId, 2).Create(),
				Enumerable.Empty<AdvisoryBoardDeferredReasonDetails>(),
				Enumerable.Empty<AdvisoryBoardDeclinedReasonDetails>(),
				Enumerable.Empty<AdvisoryBoardWithdrawnReasonDetails>(),
				timestamp, timestamp),
			new ConversionAdvisoryBoardDecision(3,_fixture.Build<AdvisoryBoardDecisionDetails>()
				.With(d => d.ConversionProjectId, 3).Create(),
				Enumerable.Empty<AdvisoryBoardDeferredReasonDetails>(),
				Enumerable.Empty<AdvisoryBoardDeclinedReasonDetails>(),
				Enumerable.Empty<AdvisoryBoardWithdrawnReasonDetails>(),
				timestamp, timestamp)
		};

		context.AddRange(seed);
		context.SaveChanges();
	}
}
