using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;

namespace Dfe.Academies.Academisation.Domain;

/// <summary>
/// Service Class to do the cross-aggregate task of submitting an Application 
/// and conditionally creating a Project
/// </summary>
public class ApplicationSubmissionService : IApplicationSubmissionService
{
	private readonly IProjectFactory _projectFactory;

	public ApplicationSubmissionService(IProjectFactory projectFactory)
	{
		_projectFactory = projectFactory;
	}

	public CommandOrCreateResult SubmitApplication(IApplication application)
	{
		var submitResult = application.Submit();

		if (submitResult is not CommandSuccessResult)
		{
			return submitResult;
		}

		if (application.ApplicationType == ApplicationType.FormAMat)
		{
			return submitResult;
		}

		var createResult = _projectFactory.Create(application);

		return createResult;
	}
}
