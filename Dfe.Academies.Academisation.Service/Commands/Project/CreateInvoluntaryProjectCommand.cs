using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Data.Repositories;
using Dfe.Academies.Academisation.Domain;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Dfe.Academies.Academisation.IService.Commands.Application;
using Dfe.Academies.Academisation.IService.Commands.Legacy.Project;
using Dfe.Academies.Academisation.Service.Mappers.Legacy.ProjectAggregate;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.Project
{
	public class CreateInvoluntaryProjectCommand : ICreateInvoluntaryProjectCommand
	{
		
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;
		private readonly IProjectFactory _projectFactory;
		

		public CreateInvoluntaryProjectCommand(IProjectCreateDataCommand projectCreateDataCommand, 
			IProjectFactory projectFactory)
		{
			_projectCreateDataCommand = projectCreateDataCommand;
			_projectFactory = projectFactory;
		}

		public async Task<CommandOrCreateResult> Execute(InvoluntaryProject project)
		{

			var domainServiceResult = _projectFactory.CreateInvoluntaryProject(project);

			switch (domainServiceResult)
			{
				case CreateValidationErrorResult createValidationErrorResult:
					return createValidationErrorResult.MapToPayloadType();
				case CreateSuccessResult<IProject> createSuccessResult:
					await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
					return domainServiceResult;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}



		}
	}
}

