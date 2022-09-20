using System.ComponentModel.DataAnnotations.Schema;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Data.ApplicationAggregate
{
	[Table(name: "ApplicationSchoolLoan")]
	public class LoanState : BaseEntity
	{
		public decimal? Amount { get; set; }

		public string? Purpose { get; set; }

		public string? Provider { get; set; }

		public decimal? InterestRate { get; set; }

		public string? Schedule { get; set; }

		public static Loan MapFromDomain(ILoan loan)
		{
			return new Loan(loan.Id, loan.Details);
		}

		public Loan MapToDomain()
		{
			return new(
				Id,
				new LoanDetails(Amount, Purpose, Provider, InterestRate, Schedule)
			);
		}
	}
}
