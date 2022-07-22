using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;
using Dfe.Academies.Academisation.Domain.Core;
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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            approvedConditionsSet,
            approvedConditionsSet ? GetRandomString : null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            approvedConditionsSet,
            approvedConditionsSet ? GetRandomString : null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        List<AdvisoryBoardDeclinedReasons> declinedReasons = declinedOtherSet
            ? new() { AdvisoryBoardDeclinedReasons.Other }
            : new() { AdvisoryBoardDeclinedReasons.Performance };

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            declinedReasons,
            declinedOtherSet ? GetRandomString : null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        List<AdvisoryBoardDeclinedReasons> declinedReasons = declinedOtherSet
            ? new() { AdvisoryBoardDeclinedReasons.Other }
            : new() { AdvisoryBoardDeclinedReasons.Performance };

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            declinedReasons,
            declinedOtherSet ? GetRandomString : null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        List<AdvisoryBoardDeferredReasons> deferredReasons = deferredOtherSet
            ? new() { AdvisoryBoardDeferredReasons.Other }
            : new() { AdvisoryBoardDeferredReasons.PerformanceConcerns };

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Deferred,
            null,
            null,
            null,
            null,
            deferredReasons,
            deferredOtherSet ? GetRandomString : null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


         List<AdvisoryBoardDeferredReasons> deferredReasons = deferredOtherSet
            ? new() { AdvisoryBoardDeferredReasons.Other }
            : new() { AdvisoryBoardDeferredReasons.PerformanceConcerns };

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Deferred,
            null,
            null,
            null,
            null,
            deferredReasons,
            deferredOtherSet ? GetRandomString : null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act 
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.Equal(details, project.AdvisoryBoardDecision?.Details);
    }

    [Fact]
    private async Task DetailsAreValid__SetsAdvisoryBoardDecisionWithExpectedProjectId()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act
        await project.AddAdvisoryBoardDecision(details);

        //Assert
        Assert.Equal(project.Id, project.AdvisoryBoardDecision?.ProjectId);
    }

    [Fact]
    private async Task AdvisoryBoardDecisionDateIsDefault___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            null,
            null,
            null,
            null,
            null,
            default,
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task AdvisoryBoardDecisionDateIsFutureDate___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsApproved_WhenApprovedConditionsSetIsNull___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            null,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            true,
            value,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task
        DecisionIsApproved__WhenApprovedConditionsSetIsFalse_AndApprovedConditionsDetailsIsNotEmpty___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            GetRandomString,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsApproved_AndDeclinedReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            null,
            new(),
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsApproved_AndDeferredReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            null,
            null,
            null,
            new(),
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_WhenDeclinedReasonsIsNull___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_WhenDeclinedReasonsIsEmpty___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            new(),
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));


        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            new() { AdvisoryBoardDeclinedReasons.Other },
            declinedOtherReason,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task
        DecisionIsDeclined__WhenDeclinedReasonsDoesNotContainOther_AndDeclinedOtherReasonIsNotEmpty___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            new() { AdvisoryBoardDeclinedReasons.Performance },
            GetRandomString,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_AndApprovedConditionsSetIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            false,
            null,
            new() { AdvisoryBoardDeclinedReasons.Performance },
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeclined_AndDeferredReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            null,
            null,
            new(),
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }


    [Fact]
    private async Task DecisionIsDeferred_WhenDeferredReasonsIsNull___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Deferred,
            null,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeferred_WhenDeferredReasonsIsEmpty___ThrowsException()
    {
        // Arrange
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Deferred,
            null,
            null,
            null,
            null,
            new(),
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

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
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Deferred,
            null,
            null,
            null,
            null,
            new() { AdvisoryBoardDeferredReasons.Other },
            declinedOtherReason,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task
        DecisionIsDeferred__WhenDeferredReasonsDoesNotContainOther_AndDeferredOtherReasonIsNotEmpty___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Deferred,
            null,
            null,
            null,
            null,
            new() { AdvisoryBoardDeferredReasons.PerformanceConcerns },
            GetRandomString,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeferred_AndApprovedConditionsSetIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            false,
            null,
            null,
            null,
            new() { AdvisoryBoardDeferredReasons.PerformanceConcerns },
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsDeferred_AndDeclinedReasonsIsNotNull___ThrowsException()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Declined,
            null,
            null,
            new(),
            null,
            new() { AdvisoryBoardDeferredReasons.PerformanceConcerns },
            null,
            DateTime.UtcNow.AddDays(-1),
            _faker.PickRandom<DecisionMadeBy>());

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => project.AddAdvisoryBoardDecision(details));
    }

    [Fact]
    private async Task DecisionIsMade_ByDirectorGeneral___ReturnsDecision()
    {
        AdvisoryBoardDecisionFactory target = new();
        var project = await target.Create(_faker.Random.Int(1, 1000));

        AdvisoryBoardDecisionDetails details = new(
            AdvisoryBoardDecisions.Approved,
            false,
            null,
            null,
            null,
            null,
            null,
            DateTime.UtcNow.AddDays(-1),
            DecisionMadeBy.DirectorGeneral);

        // Act
        await project.AddAdvisoryBoardDecision(details);

        // Assert
        Assert.Equal(DecisionMadeBy.DirectorGeneral, project.AdvisoryBoardDecision?.Details.DecisionMadeBy);
    }
}
