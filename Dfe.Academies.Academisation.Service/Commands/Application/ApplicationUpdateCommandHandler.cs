﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Application;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application;

public class ApplicationUpdateCommandHandler : IRequestHandler<ApplicationUpdateCommand, CommandResult>
{
	private readonly IApplicationRepository _applicationRepository;

	public ApplicationUpdateCommandHandler(IApplicationRepository applicationRepository)
	{
		_applicationRepository = applicationRepository;
	}

	public async Task<CommandResult> Handle(ApplicationUpdateCommand request, CancellationToken cancellationToken)
	{
		var existingApplication = await _applicationRepository.GetByIdAsync(request.ApplicationId);
		if (existingApplication is null)
		{
			return new NotFoundCommandResult();
		}

		var result = existingApplication.Update(
			request.ApplicationType,
			request.ApplicationStatus,
			request.Contributors.Select(c => new KeyValuePair<int, ContributorDetails>(c.ContributorId, c.ToDomain())),
			request.Schools.Select(s => 
				new UpdateSchoolParameter(s.Id, 
					s.TrustBenefitDetails,
					s.OfstedInspectionDetails,
					s.Safeguarding,
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
		_applicationRepository.ConcurrencySafeUpdate(existingApplication, request.Version);
		await _applicationRepository.UnitOfWork.SaveChangesAsync();
		return result;
	}
}
