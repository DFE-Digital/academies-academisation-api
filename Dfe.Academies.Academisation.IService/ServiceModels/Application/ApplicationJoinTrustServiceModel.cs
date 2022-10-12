using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record ApplicationJoinTrustServiceModel(int Id, int UKPRN, string TrustName, bool? ChangesToTrust, string? ChangesToTrustExplained, bool? ChangesToLaGovernance, string? ChangesToLaGovernanceExplained);
}
