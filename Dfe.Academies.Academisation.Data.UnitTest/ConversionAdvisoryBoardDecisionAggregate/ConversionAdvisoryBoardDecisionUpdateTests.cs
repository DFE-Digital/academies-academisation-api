using System;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
using Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Data.UnitTest.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecisionUpdateTests
{
	private readonly AcademisationContext _context;

	private readonly Fixture _fixture = new();
	private readonly Mock<IConversionAdvisoryBoardDecision> _mockDecision = new();
	private readonly AdvisoryBoardDecisionRepository _repo;
	public ConversionAdvisoryBoardDecisionUpdateTests()
	{
		_context = new TestAdvisoryBoardDecisionContext().CreateContext();
		_repo = new AdvisoryBoardDecisionRepository(_context);
	}

	[Fact]
	public async Task WhenRecordAlreadyExists___UpdatesExistingRecord()
	{
		//Arrange
		const int decisionId = 1;

		AdvisoryBoardDecisionUpdateDataCommand query = new(_repo);

		var existingDecision = await _context.ConversionAdvisoryBoardDecisions
			.SingleAsync(d => d.Id == decisionId);

		var originalDecisionApprovedConditionsSet = existingDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet;
		var originalDecisionLastModified = existingDecision.LastModifiedOn;
		var originalDecisionCreatedOn = existingDecision.CreatedOn;

		var details = _fixture.Build<AdvisoryBoardDecisionDetails>()
			.With(d => d.Decision, AdvisoryBoardDecision.Approved)
			.With(d => d.AdvisoryBoardDecisionDate, DateTime.Now.AddDays(-1))
					.With(d => d.ApprovedConditionsSet, !existingDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet)
					.Create();

		await _context.ConversionAdvisoryBoardDecisions.LoadAsync();

		existingDecision.Update(details, existingDecision.DeferredReasons, existingDecision.DeclinedReasons, existingDecision.WithdrawnReasons);

		//Act
		await query.Execute(existingDecision);

		var result = await _context.ConversionAdvisoryBoardDecisions.FindAsync(decisionId);

		//Assert
		Assert.Multiple(
			() => Assert.NotEqual(originalDecisionApprovedConditionsSet, result!.AdvisoryBoardDecisionDetails.ApprovedConditionsSet),
			() => Assert.NotEqual(originalDecisionLastModified, result!.LastModifiedOn),
			() => Assert.Equal(originalDecisionCreatedOn, result!.CreatedOn)
		);
	}
}
