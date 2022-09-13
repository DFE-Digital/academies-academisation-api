using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class LoanServiceModelMapper
	{
		internal static LoanServiceModel ToServiceModel(this LoanDetails loan)
		{
			return new(
				loan.LoanId,
				loan.Amount,
				loan.Purpose,
				loan.Provider,
				loan.InterestRate,
				loan.Schedule,
				loan.EndDate,
				loan.TermMonths
			);
		}

		internal static LoanDetails ToDomain(this LoanServiceModel loan)
		{
			return new(
				loan.LoanId,
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
