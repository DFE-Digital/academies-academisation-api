using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.IData.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class TestProjectCreateDataCommand : ITestProjectCreateDataCommand
	{
		private readonly IProjectCreateDataCommand _projectCreateDataCommand;

		public TestProjectCreateDataCommand(IProjectCreateDataCommand projectCreateDataCommand)
		{
			_projectCreateDataCommand = projectCreateDataCommand;
		}

		public async Task<IProject> Execute()
		{
			var domainServiceResult = new Project(1,new ProjectDetails()).CreateTestProject();

			switch (domainServiceResult)
			{
				case CreateSuccessResult<IProject> createSuccessResult:
					var result = await _projectCreateDataCommand.Execute(createSuccessResult.Payload);
					return result;
				default:
					throw new NotImplementedException("Other CreateResult types not expected");
			}
		}
	}
}
