using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record ApplicationJoinTrustServiceModel(int Id, int UKPRN, string TrustName, string TrustReference, ChangesToTrust? ChangesToTrust, string? ChangesToTrustExplained, bool? ChangesToLaGovernance, string? ChangesToLaGovernanceExplained);
}
