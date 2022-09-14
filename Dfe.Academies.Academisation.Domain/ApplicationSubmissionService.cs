using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.Services;

namespace Dfe.Academies.Academisation.Domain;

/// <summary>
/// Service Class to do the cross-aggregate task of submitting an Application 
/// and conditionally creating a Project
/// </summary>
public class ApplicationSubmissionService : IApplicationSubmissionService
{
	public OperationResult SubmitApplication(IApplication application)
	{
		throw new NotImplementedException();
	}
}
