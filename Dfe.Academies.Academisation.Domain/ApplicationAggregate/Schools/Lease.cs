using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools
{
	public class Lease : ILease
	{
		public int Id { get; private set; }
		public int LeaseTerm { get; private set; }

		public decimal RepaymentAmount { get; private set; }

		public decimal InterestRate { get; private set; }
		public decimal PaymentsToDate { get; private set; }

		public string Purpose { get; private set; }

		public string ValueOfAssets { get; private set; }

		public string ResponsibleForAssets { get; private set; }

		public static Lease Create(int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate,
			string purpose,
			string valueOfAssets, string responsibleForAssets)
		{
			return new Lease(0, leaseTerm, repaymentAmount, interestRate, paymentsToDate, purpose, valueOfAssets,
				responsibleForAssets);
		}

		public void Update(int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose,
			string valueOfAssets, string responsibleForAsserts)
		{
			LeaseTerm = leaseTerm;
			RepaymentAmount = repaymentAmount;
			InterestRate = interestRate;
			PaymentsToDate = paymentsToDate;
			Purpose = purpose;
			ValueOfAssets = valueOfAssets;
			ResponsibleForAssets = responsibleForAsserts;
		}

		public Lease(int id, int leaseTerm, decimal repaymentAmount, decimal interestRate, decimal paymentsToDate, string purpose, string valueOfAssets, string responsibleForAssets)
		{
			Id = id;
			LeaseTerm = leaseTerm;
			RepaymentAmount = repaymentAmount;
			InterestRate = interestRate;
			PaymentsToDate = paymentsToDate;
			Purpose = purpose;
			ValueOfAssets = valueOfAssets;
			ResponsibleForAssets = responsibleForAssets;
		}
	}
}
