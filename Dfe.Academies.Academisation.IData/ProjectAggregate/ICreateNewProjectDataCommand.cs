﻿
using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface ICreateNewProjectDataCommand
	{
		Task<CreateResult> Execute(NewProject project);
	}
}
