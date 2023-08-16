using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Commands.Application;
using MediatR;

namespace Dfe.Academies.Academisation.IService.RequestModels;

public class ApplicationDeleteCommand : IRequest<CommandResult>
{
	public ApplicationDeleteCommand(int applicationId)
	{
		ApplicationId = applicationId;	
	}

	public int ApplicationId{ get; }
}
