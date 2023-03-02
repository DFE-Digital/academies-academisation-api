using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
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
	private readonly IDateTimeProvider _dateTimeProvider;

	public ApplicationSubmissionService(IProjectFactory projectFactory, IDateTimeProvider dateTimeProvider)
	{
		_projectFactory = projectFactory;
		_dateTimeProvider = dateTimeProvider;
	}

	public CommandOrCreateResult SubmitApplication(IApplication application)
	{
		var submitResult = application.Submit(_dateTimeProvider.Now);

		if (submitResult is not CommandSuccessResult) return submitResult;
		if (application.ApplicationType is not (ApplicationType.JoinAMat or ApplicationType.FormAMat)) return submitResult;

		return application.ApplicationType is ApplicationType.FormAMat ? _projectFactory.CreateFormAMat(application) : _projectFactory.Create(application);
	}
}
