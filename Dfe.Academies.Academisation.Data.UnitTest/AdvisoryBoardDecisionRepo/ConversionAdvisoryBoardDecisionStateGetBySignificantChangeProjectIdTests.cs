using AutoFixture;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionStateGetBySignificantChangeProjectIdTests
{
	private readonly AdvisoryBoardDecisionRepository _target;
	private readonly AcademisationContext _context;
	private readonly IMediator _mediator;
	private readonly Fixture _fixture = new();

	public ConversionAdvisoryBoardDecisionStateGetBySignificantChangeProjectIdTests()
	{
		_context = new TestAdvisoryBoardDecisionContext(_mediator).CreateContext();
		_target = new AdvisoryBoardDecisionRepository(_context);
	}

	[Fact]
	public async Task WhenRecordExists_ShouldReturnExpectedDecision()
	{
		const int expectedProjectId = 777;
		var timestamp = DateTime.UtcNow;

		var details = _fixture.Build<AdvisoryBoardDecisionDetails>()
			.With(d => d.ConversionProjectId, (int?)null)
			.With(d => d.TransferProjectId, (int?)null)
			.With(d => d.SignificantChangeProjectId, expectedProjectId)
			.Create();

		var decision = new ConversionAdvisoryBoardDecision(
			99,
			details,
			new List<AdvisoryBoardDeferredReasonDetails>(),
			new List<AdvisoryBoardDeclinedReasonDetails>(),
			new List<AdvisoryBoardWithdrawnReasonDetails>(),
			new List<AdvisoryBoardDAORevokedReasonDetails>(),
			timestamp,
			timestamp);

		await _context.AddAsync(decision);
		await _context.SaveChangesAsync();

		var result = await _target.GetSignificantChangeDecision(expectedProjectId);

		Assert.Multiple(
			() => Assert.NotNull(result),
			() => Assert.IsType<ConversionAdvisoryBoardDecision>(result),
			() => Assert.Equal(expectedProjectId, result!.AdvisoryBoardDecisionDetails.SignificantChangeProjectId),
			() => Assert.NotEqual(default, result!.Id),
			() => Assert.NotEqual(default, result!.CreatedOn),
			() => Assert.NotEqual(default, result!.LastModifiedOn)
		);
	}

	[Fact]
	public async Task WhenRecordDoesNotExist_ShouldReturnNull()
	{
		const int missingProjectId = 404;

		var result = await _target.GetSignificantChangeDecision(missingProjectId);

		Assert.Null(result);
	}
}
