using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.Academies.Academisation.IDomain.Services;

public interface IApplicationSubmissionService
{
	CommandOrCreateResult SubmitApplication(IApplication application, IEnumerable<EstablishmentDto> establishmentDtos);
}
