using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.School;

public class SetAdditionalDetailsCommandHandler : IRequestHandler<SetAdditionalDetailsCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository;

	public SetAdditionalDetailsCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(SetAdditionalDetailsCommand request, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(request.ApplicationId);
		if (existingApplication == null) return new NotFoundCommandResult();

		var result = existingApplication.SetAdditionalDetails(
			request.SchoolId,
			request.TrustBenefitDetails,
			request.OfstedInspectionDetails,
			request.SafeguardingDetails,
			request.LocalAuthorityReorganisationDetails,
			request.LocalAuthorityClosurePlanDetails,
			request.DioceseName,
			request.DioceseFolderIdentifier,
			request.PartOfFederation,
			request.FoundationTrustOrBodyName,
			request.FoundationConsentFolderIdentifier,
			request.ExemptionEndDate,
			request.MainFeederSchools,
			request.ResolutionConsentFolderIdentifier,
			request.ProtectedCharacteristics,
			request.FurtherInformation);
		
		if (result is not CommandSuccessResult)
		{
			return result;
		}
			
		_applicationRepository.Update(existingApplication);
		return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(new CancellationToken()) 
			? new CommandSuccessResult()
			: new BadRequestCommandResult();
	}
}
