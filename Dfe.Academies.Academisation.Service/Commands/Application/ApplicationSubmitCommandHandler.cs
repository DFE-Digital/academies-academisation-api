﻿using System.Collections;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.Services;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Application
{
	public class ApplicationSubmitCommandHandler : IRequestHandler<ApplicationSubmitCommand, CommandOrCreateResult>
	{
		private readonly IApplicationRepository _applicationRepository;
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		private readonly IApplicationSubmissionService _applicationSubmissionService;

		public ApplicationSubmitCommandHandler(
			IApplicationRepository applicationRepository,
			IProjectCreateDataCommand projectCreateDataCommand,
			IApplicationSubmissionService applicationSubmissionService)
		{
			_applicationRepository = applicationRepository;
			_projectCreateDataCommand = projectCreateDataCommand;
			_applicationSubmissionService = applicationSubmissionService;
		}

		public async Task<CommandOrCreateResult> Handle(ApplicationSubmitCommand command, CancellationToken cancellationToken)
		{
			var application = await _applicationRepository.GetByIdAsync(command.applicationId);

			if (application is null)
			{
				return new NotFoundCommandResult();
			}

			var domainServiceResult = _applicationSubmissionService.SubmitApplication(application);
			
			switch (domainServiceResult)
			{
				case CommandValidationErrorResult:
					return domainServiceResult;
				case CreateValidationErrorResult:
					return domainServiceResult;
				case CommandSuccessResult:
					break;
				case CreateSuccessResult<IProject> createSuccessResult:
					await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
					break;
				case CreateSuccessResult<IEnumerable<IProject>> createSuccessResult:
					foreach (var project in createSuccessResult.Payload)
					{
						await _projectCreateDataCommand.Execute(project);
					}
					break;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
			
			_applicationRepository.Update(application);
			await _applicationRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

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
