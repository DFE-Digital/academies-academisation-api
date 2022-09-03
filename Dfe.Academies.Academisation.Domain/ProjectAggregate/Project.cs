using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class Project
{
	private Project(ProjectDetails projectDetails)
	{
		ProjectDetails = projectDetails;
	}

	/// <summary>
	/// This is the persistence constructor, only use from the data layer
	/// </summary>
	public Project(int id, ProjectDetails projectDetails)
	{
		Id = id;
		ProjectDetails = projectDetails;
	}

	public int Id { get; }

	public ProjectDetails ProjectDetails { get; }
}
