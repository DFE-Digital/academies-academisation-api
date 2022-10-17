using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Schools;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationSchoolLoan")]
	public class LoanState : BaseEntity
	{
		public decimal Amount { get; set; }

		public string Purpose { get; set; }

		public string Provider { get; set; }

		public decimal InterestRate { get; set; }

		public string Schedule { get; set; }

		public static Loan MapFromDomain(ILoan loan)
		{
			return new Loan(loan.Id, loan.Amount, loan.Purpose, loan.Provider, loan.InterestRate, loan.Schedule);
		}

		public Loan MapToDomain()
		{
			return new Loan(
				Id,
				Amount,
				Purpose,
				Provider,
				InterestRate,
				Schedule
			);
		}
	}
}
