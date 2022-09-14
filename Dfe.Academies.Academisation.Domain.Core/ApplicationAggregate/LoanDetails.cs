namespace Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate
{
	public record LoanDetails(
		decimal? Amount,
		string Purpose,
		string Provider,
		decimal? InterestRate,
		string? Schedule
	);
}
