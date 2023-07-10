using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public record ApplicationUpdateCommand(
	int ApplicationId,
	ApplicationType ApplicationType,
	ApplicationStatus ApplicationStatus,
	IReadOnlyCollection<ApplicationContributorServiceModel> Contributors,
	IReadOnlyCollection<ApplicationSchoolServiceModel> Schools,
	Guid Version) : IRequest<CommandResult>;
