namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

public interface ILease
{
	public int Id { get; }
	public int LeaseTerm { get; }
	public decimal RepaymentAmount { get; }
	public decimal InterestRate { get; }
	public decimal PaymentsToDate { get; }
	public string Purpose { get; }
	public string ValueOfAssets { get; }
	public string ResponsibleForAssets { get; }
		
	public void Update(int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAsserts);
}
