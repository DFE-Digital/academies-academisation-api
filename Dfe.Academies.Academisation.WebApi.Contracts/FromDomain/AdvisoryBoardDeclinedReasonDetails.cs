using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.WebApi.Contracts.FromDomain
{
	public record AdvisoryBoardDeclinedReasonDetails(AdvisoryBoardDeclinedReason Reason, string Details);
}
