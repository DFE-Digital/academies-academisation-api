using System;
using System.Threading.Tasks;
using AutoFixture;
using Dfe.Academies.Academisation.Data.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Data.UnitTest.Contexts;
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
	public async Task WhenRecordDoesNotExist___ThrowsConcurrencyException()
	{
		const int decisionId = 4;

		AdvisoryBoardDecisionUpdateDataCommand query = new(_repo);

		_mockDecision.SetupGet(d => d.Id).Returns(decisionId);

		_mockDecision.SetupGet(d => d.AdvisoryBoardDecisionDetails)
		 	.Returns(_fixture.Create<AdvisoryBoardDecisionDetails>());

		//Act & Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() => query.Execute(_mockDecision.Object));
	}

	[Fact]
	public async Task WhenRecordAlreadyExists___UpdatesExistingRecord()
	{
		//Arrange
		const int decisionId = 1;

		AdvisoryBoardDecisionUpdateDataCommand query = new(_repo);

		var existingDecision = await _context.ConversionAdvisoryBoardDecisions
			.AsNoTracking()
			.SingleAsync(d => d.Id == decisionId);

		_mockDecision
			.SetupGet(d => d.Id)
			.Returns(decisionId);

		_mockDecision
			.SetupGet(d => d.CreatedOn)
			.Returns(existingDecision.CreatedOn);

		_mockDecision
			.SetupGet(d => d.LastModifiedOn)
			.Returns(existingDecision.LastModifiedOn);

		_mockDecision.SetupGet(d => d.AdvisoryBoardDecisionDetails)
			.Returns(
				_fixture.Build<AdvisoryBoardDecisionDetails>()
					.With(d => d.ApprovedConditionsSet, !existingDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet)
					.Create());

		await _context.ConversionAdvisoryBoardDecisions.LoadAsync();

		//Act
		await query.Execute(_mockDecision.Object);

		_context.ChangeTracker.Clear();

		var result = await _context.ConversionAdvisoryBoardDecisions.FindAsync(decisionId);

		//Assert
		Assert.Multiple(
			() => Assert.NotEqual(existingDecision.AdvisoryBoardDecisionDetails.ApprovedConditionsSet, result!.AdvisoryBoardDecisionDetails.ApprovedConditionsSet),
			() => Assert.NotEqual(existingDecision.LastModifiedOn, result!.LastModifiedOn),
			() => Assert.Equal(existingDecision.CreatedOn, result!.CreatedOn)
		);
	}
}
