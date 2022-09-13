using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class LoanServiceModelMapper
	{
		internal static LoanServiceModel FromDomain(ILoan loan)
		{
			return new(
				loan.Id,
				loan.Details.Amount,
				loan.Details.Purpose,
				loan.Details.Provider,
				loan.Details.InterestRate,
				loan.Details.Schedule,
				loan.Details.EndDate,
				loan.Details.TermMonths
			);
		}

		internal static LoanDetails ToDomain(this LoanServiceModel loan)
		{
			return new(
				loan.Amount,
				loan.Purpose,
				loan.Provider,
				loan.InterestRate,
				loan.Schedule,
				loan.EndDate,
				loan.TermMonths
			);
		}
	}
}
