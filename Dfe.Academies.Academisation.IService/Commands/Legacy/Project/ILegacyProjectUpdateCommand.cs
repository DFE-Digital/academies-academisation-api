﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;

namespace Dfe.Academies.Academisation.IService.Commands.Legacy.Project;

public interface ILegacyProjectUpdateCommand
{
	Task<CommandResult> Execute(int id, LegacyProjectServiceModel legacyProjectServiceModel);
}
