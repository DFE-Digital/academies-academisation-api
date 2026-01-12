using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProjectFactory
{
	CreateResult Create(IApplication application, IEnumerable<EstablishmentDto> establishmentDtos);
	CreateResult CreateFormAMat(IApplication application, IEnumerable<EstablishmentDto> establishmentDtos);
}
