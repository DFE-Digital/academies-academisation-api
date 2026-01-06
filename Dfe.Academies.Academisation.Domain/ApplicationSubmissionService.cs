using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments;

namespace Dfe.Academies.Academisation.Domain;

/// <summary>
/// Service Class to do the cross-aggregate task of submitting an Application 
/// and conditionally creating a Project
/// </summary>
public class ApplicationSubmissionService(IProjectFactory projectFactory, IDateTimeProvider dateTimeProvider) : IApplicationSubmissionService
{ 

	public CommandOrCreateResult SubmitApplication(IApplication application, IEnumerable<EstablishmentDto> establishmentDtos)
	{
		var submitResult = application.Submit(dateTimeProvider.Now);

		if (submitResult is not CommandSuccessResult) return submitResult;
		if (application.ApplicationType is not (ApplicationType.JoinAMat or ApplicationType.FormAMat)) return submitResult;

		return application.ApplicationType is ApplicationType.FormAMat ? projectFactory.CreateFormAMat(application, establishmentDtos) : projectFactory.Create(application, establishmentDtos);
	}
}
