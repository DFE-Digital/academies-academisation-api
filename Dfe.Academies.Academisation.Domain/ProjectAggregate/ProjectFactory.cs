using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.OutsideData;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class ProjectFactory : IProjectFactory
{		
	public CreateResult<IProject> Create(IApplication application, EstablishmentDetails establishmentDetails, MisEstablishmentDetails misEstablishmentDetails)
	{
		return Project.Create(application, establishmentDetails, misEstablishmentDetails);
	}
}
