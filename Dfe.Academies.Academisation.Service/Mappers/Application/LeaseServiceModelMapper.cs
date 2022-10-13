using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Mappers.Application
{
	internal static class LeaseServiceModelMapper
	{
		internal static LeaseServiceModel ToServiceModel(this ILease lease)
		{
			return new(
				lease.Id,
				lease.LeaseTerm,
				lease.RepaymentAmount,
				lease.InterestRate,
				lease.PaymentsToDate,
				lease.Purpose,
				lease.ValueOfAssets,
				lease.ResponsibleForAssets
			);
		}

		internal static LeaseDetails ToDomain(this LeaseServiceModel lease)
		{
			return new(
				lease.leaseTerm,
				lease.repaymentAmount,
				lease.interestRate,
				lease.paymentsToDate,
				lease.purpose,
				lease.valueOfAssets,
				lease.responsibleForAssets
			);
		}
	}
}
