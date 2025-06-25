using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.Domain.Summary
{
	public interface ISummaryDataService
	{
		Task<IEnumerable<ProjectSummary>> GetProjectSummariesByAssignedEmail(string email, bool includeConversions, bool includeTransfers, bool includeFormAMat);
	}
}
