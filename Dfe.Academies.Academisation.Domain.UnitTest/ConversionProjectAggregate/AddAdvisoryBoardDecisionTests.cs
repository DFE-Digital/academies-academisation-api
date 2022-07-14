using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;
using Xunit;
using ValidationException = FluentValidation.ValidationException;


namespace Dfe.Academies.Academisation.Domain.UnitTest.ConversionProjectAggregate;

public class AddAdvisoryBoardDecisionTests
{
    private readonly Faker _faker = new();

    private string GetRandomString => _faker.Random.String(1, 20, '\u0020', '\u007f');

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsApproved_AndDetailsAreValid___SetsAdvisoryBoardDecision(bool approvedConditionsSet)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        
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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.IsType<AdvisoryBoardDecision>(project.AdvisoryBoardDecision);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsApproved_AndDetailsAreValid___SetsAdvisoryBoardDecisionWithExpectedDetails(
        bool approvedConditionsSet)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        
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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.Equal(details, project.AdvisoryBoardDecision!.Details);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeclined_AndDetailsAreValid___SetsAdvisoryBoardDecision(bool declinedOtherSet)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.IsType<AdvisoryBoardDecision>(project.AdvisoryBoardDecision);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeclined_AndDetailsAreValid__SetsAdvisoryBoardDecisionWithExpectedDetails(
        bool declinedOtherSet)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.Equal(details, project.AdvisoryBoardDecision?.Details);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeferred_AndDetailsAreValid___SetsAdvisoryBoardDecision(bool deferredOtherSet)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.IsType<AdvisoryBoardDecision>(project.AdvisoryBoardDecision);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    private async Task DecisionIsDeferred_AndDetailsAreValid__SetsAdvisoryBoardDecisionWithExpectedDetails(
        bool deferredOtherSet)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.Equal(details, project.AdvisoryBoardDecision?.Details);
    }

    [Fact]
    private async Task DetailsAreValid__SetsAdvisoryBoardDecisionWithExpectedProjectId()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.Equal(project.Id, project.AdvisoryBoardDecision?.ProjectId);
    }
    
    [Fact]
    private async Task AdvisoryBoardDecisionDateIsDefault___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Approved,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: null,
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: default);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task AdvisoryBoardDecisionDateIsFutureDate___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsApproved_WhenApprovedConditionsSetIsNull___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    private async Task
        DecisionIsApproved__WhenApprovedConditionsSetIsTrue_AndApprovedConditionsDetailsIsEmpty___ThrowsException(
            string? value)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));  
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task
        DecisionIsApproved__WhenApprovedConditionsSetIsFalse_AndApprovedConditionsDetailsIsNotEmpty___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsApproved_AndDeclinedReasonsIsNotNull___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsApproved_AndDeferredReasonsIsNotNull___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_WhenDeclinedReasonsIsNull___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_WhenDeclinedReasonsIsEmpty___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));  
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    private async Task
        DecisionIsDeclined__WhenDeclinedReasonsContainsOther_AndDeclinedOtherReasonIsEmpty___ThrowsException(
            string declinedOtherReason)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task
        DecisionIsDeclined__WhenDeclinedReasonsDoesNotContainOther_AndDeclinedOtherReasonIsNotEmpty___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_AndApprovedConditionsSetIsNotNull___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_AndDeferredReasonsIsNotNull___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }


    [Fact]
    private async Task DecisionIsDeferred_WhenDeferredReasonsIsNull___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeferred_WhenDeferredReasonsIsEmpty___ThrowsException()
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    private async Task
        DecisionIsDeferred__WhenDeferredReasonsContainsOther_AndDeferredOtherReasonIsEmpty___ThrowsException(
            string declinedOtherReason)
    {
        // Arrange
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task
        DecisionIsDeferred__WhenDeferredReasonsDoesNotContainOther_AndDeferredOtherReasonIsNotEmpty___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

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
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeferred_AndApprovedConditionsSetIsNotNull___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: false,
            ApprovedConditionsDetails: null,
            DeclinedReasons: null,
            DeclinedOtherReason: null,
            DeferredReasons: new() {AdvisoryBoardDeferredReasons.PerformanceConcerns},
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeferred_AndDeclinedReasonsIsNotNull___ThrowsException()
    {
        ConversionProjectFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));
        

        AdvisoryBoardDecisionDetails details = new(
            Decision: AdvisoryBoardDecisions.Declined,
            ApprovedConditionsSet: null,
            ApprovedConditionsDetails: null,
            DeclinedReasons: new(),
            DeclinedOtherReason: null,
            DeferredReasons: new() {AdvisoryBoardDeferredReasons.PerformanceConcerns},
            DeferredOtherReason: null,
            AdvisoryBoardDecisionDate: DateTime.UtcNow.AddDays(-1));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }
}