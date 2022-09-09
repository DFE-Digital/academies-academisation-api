using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class Project : IProject
{
	private Project(ProjectDetails projectDetails)
	{
		Details = projectDetails;
	}

	/// <summary>
	/// This is the persistence constructor, only use from the data layer
	/// </summary>
	public Project(int id, ProjectDetails projectDetails)
	{
		Id = id;
		Details = projectDetails;
	}

	public int Id { get; }

	public ProjectDetails Details { get; }

	public static CreateResult<IProject> Create(IApplication application)
	{
		if (application.ApplicationType != Core.ApplicationAggregate.ApplicationType.JoinAMat)
		{
			return new CreateValidationErrorResult<IProject>(
				new List<ValidationError>
				{
					new ValidationError("ApplicationStatus", "Only projects of type JoinAMat are supported")
				});
		}

		var projectDetails = new ProjectDetails(
			application.Schools.Single().Details.Urn,
			// TODO: speak to Martin about what this maps from
			0);

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}
}
