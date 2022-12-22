using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools
{
	public class Loan : DynamicsSchoolLoanEntity, ILoan
	{
		protected Loan() { }
		public Loan(int id, decimal amount, string purpose, string provider, decimal interestRate, string schedule)
		{
			Id = id;
			Amount = amount;
			Purpose = purpose;
			Provider = provider;
			InterestRate = interestRate;
			Schedule = schedule;
		}

		public decimal Amount { get; private set; }
		public string Purpose { get; private set; }
		public string Provider { get; private set; }
		public decimal InterestRate { get; private set; }
		public string Schedule { get; private set; }
		public int Id { get; private set; }
		
		public static Loan Create(decimal amount, string purpose, string provider, decimal interestRate, string schedule)
		{
			return new Loan(0, amount, purpose, provider, interestRate, schedule);
		}

		public void Update(decimal amount, string purpose, string provider, decimal interestRate, string schedule)
		{
			Amount = amount;
			Purpose = purpose;
			Provider = provider;
			InterestRate = interestRate;
			Schedule = schedule;
		}
	}
}
