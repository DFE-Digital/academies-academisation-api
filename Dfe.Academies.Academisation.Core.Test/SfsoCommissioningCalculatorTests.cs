namespace Dfe.Academies.Academisation.Core.Test;

public class SfsoCommissioningCalculatorTests
{
	[Fact]
	public void CalculateRequestedDate_ReturnsNull_WhenProposedDateIsNull()
	{
		Assert.Null(SfsoCommissioningCalculator.CalculateRequestedDate(null, true));
	}

	[Fact]
	public void CalculateRequestedDate_ReturnsProposedMinus15_WhenMoreThan15DaysAway()
	{
		var proposed = DateTime.Today.AddDays(40);

		var result = SfsoCommissioningCalculator.CalculateRequestedDate(proposed, true);

		Assert.Equal(proposed.AddDays(-15), result);
	}

	[Fact]
	public void CalculateRequestedDate_ReturnsNull_WhenWithin15Days()
	{
		var proposed = DateTime.Today.AddDays(10);

		var result = SfsoCommissioningCalculator.CalculateRequestedDate(proposed, true);

		Assert.Null(result);
	}

	[Fact]
	public void CalculateRequestedDate_ReturnsNull_WhenProposedIsToday()
	{
		Assert.Null(SfsoCommissioningCalculator.CalculateRequestedDate(DateTime.Today, true));
	}

	[Fact]
	public void CalculateRequestedDate_ReturnsNull_WhenProposedIsInThePast()
	{
		Assert.Null(SfsoCommissioningCalculator.CalculateRequestedDate(DateTime.Today.AddDays(-5), true));
	}
}