using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project
{
	public interface IMaintainTestProjectCommand
	{
		Task<CreateSuccessResult<TestProjectModel>> Execute();
	}
}
