using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class LoanServiceModelMapper
	{
		internal static LoanServiceModel ToServiceModel(this ILoan loan)
		{
			return new(
				loan.Id, 
				loan.Amount,
				loan.Purpose,
				loan.Provider,
				loan.InterestRate,
				loan.Schedule
			);
		}

		internal static LoanDetails ToDomain(this LoanServiceModel loan)
		{
			return new(
				loan.Amount,
				loan.Purpose,
				loan.Provider,
				loan.InterestRate,
				loan.Schedule
			);
		}
	}
}
