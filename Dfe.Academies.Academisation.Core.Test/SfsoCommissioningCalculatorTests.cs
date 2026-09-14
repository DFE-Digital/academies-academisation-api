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
	public void CalculateRequestedDate_ReturnsToday_WhenWithin15Days()
	{
		var proposed = DateTime.Today.AddDays(10);

		var result = SfsoCommissioningCalculator.CalculateRequestedDate(proposed, true);

		Assert.Equal(DateTime.Today, result);
	}

	[Fact]
	public void CalculateRequestedDate_ReturnsToday_WhenProposedIsToday()
	{
		Assert.Equal(DateTime.Today, SfsoCommissioningCalculator.CalculateRequestedDate(DateTime.Today, true));
	}

	[Fact]
	public void CalculateRequestedDate_ReturnsToday_WhenProposedIsInThePast()
	{
		Assert.Equal(DateTime.Today, SfsoCommissioningCalculator.CalculateRequestedDate(DateTime.Today.AddDays(-5), true));
	}
}