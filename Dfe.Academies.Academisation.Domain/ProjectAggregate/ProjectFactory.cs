using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class ProjectFactory : IProjectFactory
{
	public CreateResult Create(IApplication application, IEnumerable<EstablishmentDto> establishmentDtos)
	{
		return Project.Create(application, establishmentDtos);
	}
	public CreateResult CreateFormAMat(IApplication application, IEnumerable<EstablishmentDto> establishmentDtos)
	{
		return Project.CreateFormAMat(application, establishmentDtos);
	}
}
