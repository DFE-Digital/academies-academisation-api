using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.RequestModels;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class ApplicationDeleteCommandHandler : IRequestHandler<ApplicationDeleteCommand, CommandResult>
{
	
	private readonly IApplicationRepository _applicationRepository;
	

	public ApplicationDeleteCommandHandler( IApplicationRepository applicationRepository)
	{
		
		_applicationRepository = applicationRepository;
		
	}

	public async Task<CommandResult> Handle(ApplicationDeleteCommand request, CancellationToken cancellationToken)
	{
		var existingApplication= await _applicationRepository.GetByIdAsync(request.ApplicationId);
		if (existingApplication is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingApplication.ValidateSoftDelete(existingApplication.ApplicationId);
			
        if (result is CommandValidationErrorResult)
		{
			return result;
		}
		if (result is not CommandSuccessResult)
		{
			throw new NotImplementedException();
		}
		await _applicationRepository.SoftDelete(existingApplication.ApplicationId);
		await _applicationRepository.UnitOfWork.SaveChangesAsync();
		return result;

	}
}
