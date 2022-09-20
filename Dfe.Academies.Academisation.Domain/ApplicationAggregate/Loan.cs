using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate
{
	public class Loan : ILoan
	{
		public Loan(int id, LoanDetails details)
		{
			Id = id;
			Details = details;
		}

		public int Id { get; internal set; }

		public LoanDetails Details { get; }
	}
}
