using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;
using Xunit;
using ValidationException = FluentValidation.ValidationException;


namespace Dfe.Academies.Academisation.Domain.UnitTest.AdvisoryBoardDecisionAggregate;

public class AdvisoryBoardDecisionCreateTests
{
    private readonly Faker _faker = new();

    private string GetRandomString => _faker.Random.String(1, 20, '\u0020', '\u007f');
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsApproved_AndDetailsAreValid___ReturnsAdvisoryBoardDecision(bool approvedConditionsSet)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: approvedConditionsSet,
            ApprovedConditionsDetails: approvedConditionsSet ? GetRandomString : null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(_faker.Random.Int(), details);
        
        //Assert
        Assert.IsType<AdvisoryBoardDecision>(result);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsApproved_AndDetailsAreValid__ReturnsAdvisoryBoardDecisionWithExpectedDetails(bool approvedConditionsSet)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: approvedConditionsSet,
            ApprovedConditionsDetails: approvedConditionsSet ? GetRandomString : null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(_faker.Random.Int(), details);
        
        //Assert
        Assert.Equal(details, result.Details);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeclined_AndDetailsAreValid___ReturnsAdvisoryBoardDecision(bool declinedOtherSet)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();

        List<AdvisoryBoardDeclinedReasons> declinedReasons = declinedOtherSet
            ? new() {AdvisoryBoardDeclinedReasons.Other}
            : new() {AdvisoryBoardDeclinedReasons.Performance};
        
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: declinedReasons,
            DeclinedOtherReason: declinedOtherSet ? GetRandomString : null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(_faker.Random.Int(), details);
        
        //Assert
        Assert.IsType<AdvisoryBoardDecision>(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeclined_AndDetailsAreValid__ReturnsAdvisoryBoardDecisionWithExpectedDetails(bool declinedOtherSet)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        
        List<AdvisoryBoardDeclinedReasons> declinedReasons = declinedOtherSet
            ? new() {AdvisoryBoardDeclinedReasons.Other}
            : new() {AdvisoryBoardDeclinedReasons.Performance};
        
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: declinedReasons,
            DeclinedOtherReason: declinedOtherSet ? GetRandomString : null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(_faker.Random.Int(), details);
        
        //Assert
        Assert.Equal(details, result.Details);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeferred_AndDetailsAreValid___ReturnsAdvisoryBoardDecision(bool deferredOtherSet)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();

        List<AdvisoryBoardDeferredReasons> deferredReasons = deferredOtherSet
            ? new() {AdvisoryBoardDeferredReasons.Other}
            : new() {AdvisoryBoardDeferredReasons.PerformanceConcerns};
        
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Deferred,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: deferredReasons,
            DeferredOtherReason: deferredOtherSet ? GetRandomString : null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(_faker.Random.Int(), details);
        
        //Assert
        Assert.IsType<AdvisoryBoardDecision>(result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeferred_AndDetailsAreValid__ReturnsAdvisoryBoardDecisionWithExpectedDetails(bool deferredOtherSet)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        
        List<AdvisoryBoardDeferredReasons> deferredReasons = deferredOtherSet
            ? new() {AdvisoryBoardDeferredReasons.Other}
            : new() {AdvisoryBoardDeferredReasons.PerformanceConcerns};
        
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Deferred,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: deferredReasons,
            DeferredOtherReason: deferredOtherSet ? GetRandomString : null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(_faker.Random.Int(), details);
        
        //Assert
        Assert.Equal(details, result.Details);
    }
    
    [Fact]
    private async Task DetailsAreValid__ReturnsAdvisoryBoardDecisionWithExpectedProjectIdSet()
    {
        // Arrange
        var projectId = _faker.Random.Int();
        
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act 
        var result = await target.Create(projectId, details);
        
        //Assert
        Assert.Equal(projectId, result.ProjectId);
    }
    
    [Fact]
    private async Task AdvisoryBoardDecisionDateIsDefault___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: _faker.PickRandom<AdvisoryBoardDecisions>(),
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: default);
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }

    [Fact]
    private async Task AdvisoryBoardDecisionDateIsFutureDate___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }

    [Fact]
    private async Task DecisionIsApproved_WhenApprovedConditionsSetIsNull___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    private async Task DecisionIsApproved__WhenApprovedConditionsSetIsTrue_AndApprovedConditionsDetailsIsEmpty___ThrowsException(string? value)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: true,
            ApprovedConditionsDetails: value,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }

    [Fact]
    private async Task DecisionIsApproved__WhenApprovedConditionsSetIsFalse_AndApprovedConditionsDetailsIsNotEmpty___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: GetRandomString,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }

    [Fact]
    private async Task DecisionIsApproved_AndDeclinedReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new(),
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsApproved_AndDeferredReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: new(),
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeclined_WhenDeclinedReasonsIsNull___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeclined_WhenDeclinedReasonsIsEmpty___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new(),
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    private async Task DecisionIsDeclined__WhenDeclinedReasonsContainsOther_AndDeclinedOtherReasonIsEmpty___ThrowsException(string declinedOtherReason)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new() {AdvisoryBoardDeclinedReasons.Other},
            DeclinedOtherReason: declinedOtherReason,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeclined__WhenDeclinedReasonsDoesNotContainOther_AndDeclinedOtherReasonIsNotEmpty___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new() {AdvisoryBoardDeclinedReasons.Performance},
            DeclinedOtherReason: GetRandomString,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeclined_AndApprovedConditionsSetIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new() {AdvisoryBoardDeclinedReasons.Performance},
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeclined_AndDeferredReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: new(),
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    
    
    
    
    
    [Fact]
    private async Task DecisionIsDeferred_WhenDeferredReasonsIsNull___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Deferred,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeferred_WhenDeferredReasonsIsEmpty___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Deferred,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: new(),
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    private async Task DecisionIsDeferred__WhenDeferredReasonsContainsOther_AndDeferredOtherReasonIsEmpty___ThrowsException(string declinedOtherReason)
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Deferred,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: new() {AdvisoryBoardDeferredReasons.Other},
            DeferredOtherReason: declinedOtherReason,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeferred__WhenDeferredReasonsDoesNotContainOther_AndDeferredOtherReasonIsNotEmpty___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Deferred,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: new() {AdvisoryBoardDeferredReasons.PerformanceConcerns},
            DeferredOtherReason: GetRandomString,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeferred_AndApprovedConditionsSetIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons:  new() {AdvisoryBoardDeferredReasons.PerformanceConcerns},
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
    
    [Fact]
    private async Task DecisionIsDeferred_AndDeclinedReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new(),
            DeclinedOtherReason: null,
            DeferredReasons:  new() {AdvisoryBoardDeferredReasons.PerformanceConcerns},
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));
        
        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => target.Create(_faker.Random.Int(), details));
    }
}