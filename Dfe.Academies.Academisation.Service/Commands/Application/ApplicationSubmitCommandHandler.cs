using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Query;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Queries;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class ApplicationSubmitCommandHandler(
		IApplicationRepository applicationRepository,
		IConversionProjectRepository conversionProjectRepository,
		IApplicationSubmissionService applicationSubmissionService,
		IAcademiesQueryService academiesQueryService) : IRequestHandler<ApplicationSubmitCommand, CommandOrCreateResult>
	{
		public async Task<CommandOrCreateResult> Handle(ApplicationSubmitCommand command, CancellationToken cancellationToken)
		{
			var application = await applicationRepository.GetByIdAsync(command.applicationId);

			if (application is null)
			{
				return new NotFoundCommandResult();
			}
			var urns = application.Schools.Select(s => s.Details.Urn);
			
			var establishmentDtos = await academiesQueryService.PostBulkEstablishmentsByUrns(urns);
			var domainServiceResult = applicationSubmissionService.SubmitApplication(application, establishmentDtos);
			
			switch (domainServiceResult)
			{
				case CommandValidationErrorResult:
					return domainServiceResult;
				case CreateValidationErrorResult:
					return domainServiceResult;
				case CommandSuccessResult:
					break;
				case CreateSuccessResult<IProject> createSuccessResult:
					conversionProjectRepository.Insert(createSuccessResult.Payload as Project);
					await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
					break;
				case CreateSuccessResult<IEnumerable<IProject>> createSuccessResult:
					foreach (var project in createSuccessResult.Payload)
					{
						conversionProjectRepository.Insert(project as Project);
					}
					await conversionProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
					break;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
			
			applicationRepository.Update(application);
			await applicationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
			
			switch (domainServiceResult)
			{
				case CommandResult:
					return domainServiceResult;
				case CreateValidationErrorResult createValidationErrorResult:
					return createValidationErrorResult.MapToPayloadType();
				case CreateSuccessResult<IProject> createSuccessResult:
					return createSuccessResult.MapToPayloadType(p => p.MapToServiceModel());
				case CreateSuccessResult<IEnumerable<IProject>> createSuccessResult:
					var projects = createSuccessResult.Payload.Select(p => p.MapToServiceModel());
					return new CreateSuccessResult<IEnumerable<ConversionProjectServiceModel>>(projects);
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
		}
	}
}
