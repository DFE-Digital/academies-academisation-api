using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class FormTrustCommandHandler : ISetFormTrustDetailsCommandHandler
	{

		private readonly IApplicationGetDataQuery _applicationGetDataQuery;
		private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

		public FormTrustCommandHandler(IApplicationGetDataQuery applicationGetDataQuery, IApplicationUpdateDataCommand applicationUpdateDataCommand)
		{
			_applicationGetDataQuery = applicationGetDataQuery;
			_applicationUpdateDataCommand = applicationUpdateDataCommand;
		}

		public async Task<CommandResult> Handle(int applicationId, SetFormTrustDetailsCommand command)
		{
			var existingApplication = await _applicationGetDataQuery.Execute(applicationId);
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
				command.FormTrustImprovementApprovedSponsor));

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
