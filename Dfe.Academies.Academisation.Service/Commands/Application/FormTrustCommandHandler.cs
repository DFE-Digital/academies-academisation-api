using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class FormTrustCommandHandler : IRequestHandler<SetFormTrustDetailsCommand, CommandResult>
	{

		private readonly IApplicationRepository _applicationRepository;
		private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

		public FormTrustCommandHandler(IApplicationRepository applicationRepository, IApplicationUpdateDataCommand applicationUpdateDataCommand)
		{
			_applicationRepository = applicationRepository;
			_applicationUpdateDataCommand = applicationUpdateDataCommand;
		}

		public async Task<CommandResult> Handle(SetFormTrustDetailsCommand command, CancellationToken cancellationToken)
		{
			// cancellation token will be passed down to the database requests when the repository pattern is brought in
			// shouldn't be anything that is long running just found it a good habit with async

			var existingApplication = await _applicationRepository.GetByIdAsync(command.applicationId);
			if (existingApplication is null)
			{
				return new NotFoundCommandResult();
			}

			var result = existingApplication.SetFormTrustDetails(new FormTrustDetails(
				command.FormTrustOpeningDate,
				command.FormTrustProposedNameOfTrust,
				command.TrustApproverName,
				command.TrustApproverEmail,
				command.FormTrustReasonApprovaltoConvertasSAT,
				command.FormTrustReasonApprovedPerson,
				command.FormTrustReasonForming,
				command.FormTrustReasonVision,
				command.FormTrustReasonGeoAreas,
				command.FormTrustReasonFreedom,
				command.FormTrustReasonImproveTeaching,
				command.FormTrustPlanForGrowth,
				command.FormTrustPlansForNoGrowth,
				command.FormTrustGrowthPlansYesNo,
				command.FormTrustImprovementSupport,
				command.FormTrustImprovementStrategy,
				command.FormTrustImprovementApprovedSponsor
				));

			if (result is CommandValidationErrorResult)
			{
				return result;
			}
			if (result is not CommandSuccessResult)
			{
				throw new NotImplementedException();
			}

			await _applicationUpdateDataCommand.Execute(existingApplication);

			return result;
		}

	}
}
