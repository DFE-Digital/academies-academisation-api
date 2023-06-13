using AutoMapper;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application.Trust
{
	public class CreateTrustKeyPersonCommandHandler : IRequestHandler<CreateTrustKeyPersonCommand, CommandResult>
	{
		private readonly IApplicationRepository _applicationRepository;
		private readonly IMapper _mapper;

		public CreateTrustKeyPersonCommandHandler(IApplicationRepository applicationRepository, IMapper mapper)
		{
			_applicationRepository = applicationRepository;
			_mapper = mapper;
		}

		public async Task<CommandResult> Handle(CreateTrustKeyPersonCommand command, CancellationToken cancellationToken)
		{
			// cancellation token will be passed down to the database requests when the repository pattern is brought in
			// shouldn't be anything that is long running just found it a good habit with async

			var existingApplication = await _applicationRepository.GetByIdAsync(command.ApplicationId);
			if (existingApplication is null)
			{
				return new NotFoundCommandResult();
			}

			var result = existingApplication.AddTrustKeyPerson(command.Name, command.DateOfBirth, command.Biography, command.Roles.Select(x => TrustKeyPersonRole.Create(x.Role, x.TimeInRole)));

			if (result is CommandValidationErrorResult)
			{
				return result;
			}

			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}

			_applicationRepository.Update(existingApplication);
			return await _applicationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken)
				? new CommandSuccessResult()
				: new BadRequestCommandResult();
		}

	}
}
