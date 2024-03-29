﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ProjectAggregate;

public class ProjectFactory : IProjectFactory
{
	public CreateResult Create(IApplication application)
	{
		return Project.Create(application);
	}
	public CreateResult CreateFormAMat(IApplication application)
	{
		return Project.CreateFormAMat(application);
	}
}
