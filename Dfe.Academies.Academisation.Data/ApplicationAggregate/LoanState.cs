using System.ComponentModel.DataAnnotations.Schema;
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

		public static LoanState MapFromDomain(ILoan loan)
		{
			return new()
			{
				Id = loan.Id,
				Amount = loan.Details.Amount,
				Purpose = loan.Details.Purpose,
				Provider = loan.Details.Provider,
				InterestRate = loan.Details.InterestRate,
				Schedule = loan.Details.Schedule
			};
		}
	}
}
