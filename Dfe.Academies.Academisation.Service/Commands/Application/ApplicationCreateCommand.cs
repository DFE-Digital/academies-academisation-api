using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using MediatR;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationCreateCommand : IRequest<CreateResult>
{
	public ApplicationCreateCommand(ApplicationType applicationType, ContributorRequestModel contributor)
	{
		ApplicationType = applicationType;
		Contributor = contributor;
	}

	public ApplicationType ApplicationType { get; }
	public ContributorRequestModel Contributor { get; }
}
