namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ILoan
	{
		decimal Amount { get; }
		string Purpose { get; }
		string Provider { get; }
		decimal InterestRate { get; }
		string Schedule { get; }
		int Id { get; }
		public void Update(decimal amount, string purpose, string provider, decimal interestRate, string schedule);
		
		//TODO: Remove this after refactoring away interfaces
	}
}
