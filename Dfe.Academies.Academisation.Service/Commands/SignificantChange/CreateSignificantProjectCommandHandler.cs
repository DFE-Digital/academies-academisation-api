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
			}

			string trustName = trust?.Name ?? string.Empty;
			
			var significantChangeProject = SignificantChangeProject.Create(
				command.Urn,
				command.Tier,
				trustName,
				command.TrustUkprn,
				command.Route,
				string.Empty,
				dateTimeProvider.Now);

			significantChangeProjectRepository.Insert(significantChangeProject);
			await significantChangeProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

			return new CreateSuccessResult<SignificantChangeProjectResponse>(new SignificantChangeProjectResponse
			{
				Id = significantChangeProject.Id,
				Urn = significantChangeProject.Urn,
				Tier = significantChangeProject.Tier,
				TrustName = significantChangeProject.TrustName,
				TrustUkprn = significantChangeProject.TrustUkprn,
				TypeOfSignificantChange = significantChangeProject.TypeOfSignificantChange,
				Status = significantChangeProject.Status.ToString()
			});
		}

	}
}
