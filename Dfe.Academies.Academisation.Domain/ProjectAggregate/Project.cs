using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.OutsideData;
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

	public static CreateResult<IProject> Create(IApplication application, EstablishmentDetails establishmentDetails, MisEstablishmentDetails misEstablishmentDetails)
	{
		if (application.ApplicationType != Core.ApplicationAggregate.ApplicationType.JoinAMat)
		{
			return new CreateValidationErrorResult<IProject>(
				new List<ValidationError>
				{
					new ValidationError("ApplicationStatus", "Only projects of type JoinAMat are supported")
				});
		}

		var school = application.Schools.Single().Details;

		var projectDetails = new ProjectDetails(
			school.Urn,
			misEstablishmentDetails.Laestab,
			school.SchoolName
		)
		{ UkPrn = establishmentDetails.Ukprn };

		return new CreateSuccessResult<IProject>(new Project(projectDetails));
	}
}
