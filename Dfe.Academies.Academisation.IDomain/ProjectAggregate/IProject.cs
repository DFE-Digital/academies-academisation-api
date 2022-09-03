using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IDomain.ProjectAggregate;

public interface IProject
{
	public int Id { get; }

	public ProjectDetails ProjectDetails { get; }
}
