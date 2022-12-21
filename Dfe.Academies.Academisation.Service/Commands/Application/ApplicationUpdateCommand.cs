using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ApplicationAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.RequestModels;
using Dfe.Academies.Academisation.Service.Mappers.Application;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class ApplicationUpdateCommand : IApplicationUpdateCommand
{
	private readonly IApplicationRepository _applicationRepository;
	private readonly IApplicationUpdateDataCommand _applicationUpdateDataCommand;

	public ApplicationUpdateCommand(IApplicationRepository applicationRepository, IApplicationUpdateDataCommand applicationUpdateDataCommand)
	{
		_applicationRepository = applicationRepository;
		_applicationUpdateDataCommand = applicationUpdateDataCommand;
	}

	public async Task<CommandResult> Execute(int applicationId, ApplicationUpdateRequestModel applicationServiceModel)
	{
		if (applicationId != applicationServiceModel.ApplicationId)
		{
			return new CommandValidationErrorResult(
				new List<ValidationError>() {
					new ValidationError("Id", "Ids must be the same")
				});
		}

		var existingApplication = await _applicationRepository.GetByIdAsync(applicationId);
		if (existingApplication is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingApplication.Update(
			applicationServiceModel.ApplicationType,
			applicationServiceModel.ApplicationStatus,
			applicationServiceModel.Contributors.Select(c => new KeyValuePair<int, ContributorDetails>(c.ContributorId, c.ToDomain())),
			applicationServiceModel.Schools.Select(s => 
				new UpdateSchoolParameter(s.Id, 
					s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.SafeguardingDetails,
					s.LocalAuthorityReorganisationDetails,
					s.LocalAuthorityClosurePlanDetails,
					s.DioceseName,
					s.DioceseFolderIdentifier,
					s.PartOfFederation,
					s.FoundationTrustOrBodyName,
					s.FoundationConsentFolderIdentifier,
					s.ExemptionEndDate,
					s.MainFeederSchools,
					s.ResolutionConsentFolderIdentifier,
					s.ProtectedCharacteristics,
					s.FurtherInformation,
					s.ToDomain(), 
					new List<KeyValuePair<int, LoanDetails>>(s.Loans.Select(l=> new KeyValuePair<int,LoanDetails>(l.LoanId, l.ToDomain()))),
					new List<KeyValuePair<int, LeaseDetails>>(s.Leases.Select(l => new KeyValuePair<int, LeaseDetails>(l.LeaseId, l.ToDomain()))),
					s.HasLoans,
					s.HasLeases)));
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
