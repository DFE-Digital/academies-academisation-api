﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Project;

public interface ILegacyProjectUpdateCommand
{
	Task<CommandResult> Execute(LegacyProjectServiceModel legacyProjectServiceModel);
}
