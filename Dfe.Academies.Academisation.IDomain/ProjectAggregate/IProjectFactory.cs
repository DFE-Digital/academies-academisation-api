using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.OutsideData;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProjectFactory
{
	CreateResult<IProject> Create(IApplication application, EstablishmentDetails establishmentDetails, MisEstablishmentDetails misEstablishmentDetails);
}
