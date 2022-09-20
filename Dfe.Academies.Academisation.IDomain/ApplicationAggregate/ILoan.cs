using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ApplicationAggregate
{
	public interface ILoan
	{
		public int Id { get; }

		public LoanDetails Details { get; }
	}
}
