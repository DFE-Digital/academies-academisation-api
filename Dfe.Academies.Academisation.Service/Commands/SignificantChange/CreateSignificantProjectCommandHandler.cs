using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Core.Utils;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Commands.SignificantChange
{
	public class CreateSignificantProjectCommandHandler(ISignificantChangeProjectRepository significantChangeProjectRepository, IAcademiesQueryService academiesQueryService, IDateTimeProvider dateTimeProvider, ILogger<CreateSignificantProjectCommandHandler> logger)
		: IRequestHandler<CreateSignificantProjectCommand, CreateResult>
	{
		public async Task<CreateResult> Handle(CreateSignificantProjectCommand command, CancellationToken cancellationToken)
		{

			var trust = await academiesQueryService.GetTrust(command.TrustUkprn);
			if (trust is null)
			{
				logger.LogWarning("Trust with UKPRN {Ukprn} not found", command.TrustUkprn);
				return new CreateValidationErrorResult([new ValidationError("TrustUkprn", "Trust with UKPRN {Ukprn} not found")]);
			}

			var establishment = await academiesQueryService.GetEstablishment(command.Urn);
			if (establishment is null)
			{
				logger.LogWarning("School with URN {Urn} not found", command.Urn);
				return new CreateValidationErrorResult([new ValidationError("Urn", $"School with URN {command.Urn} not found")]);
			}
			
			var significantChangeProject = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					command.Urn,
					command.Tier,
					trust.Name,
					command.TrustUkprn,
					command.Route,
					establishment.Name,
					establishment.LocalAuthorityName,
					trust.CompaniesHouseNumber),
				dateTimeProvider.Now);

			significantChangeProjectRepository.Insert(significantChangeProject);
			await significantChangeProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CreateSuccessResult<SignificantChangeProjectResponse>(new SignificantChangeProjectResponse
			{
				Id = significantChangeProject.Id,
				Urn = significantChangeProject.Urn,
				SchoolName = significantChangeProject.SchoolName,
				Tier = significantChangeProject.Tier,
				TrustName = significantChangeProject.TrustName,
				TrustUkprn = significantChangeProject.TrustUkprn,
				TypeOfSignificantChange = significantChangeProject.TypeOfSignificantChange,
				Status = significantChangeProject.Status.ToString(),
				LocalAuthorityName = significantChangeProject.LocalAuthorityName,
				CompaniesHouseNumber = significantChangeProject.CompaniesHouseNumber
			});
		}

	}
}
