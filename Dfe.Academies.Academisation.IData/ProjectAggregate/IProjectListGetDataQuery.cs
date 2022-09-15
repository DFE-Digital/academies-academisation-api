﻿using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectListGetDataQuery
	{
		Task<IEnumerable<IProject>> SearchProjects(
			List<string>? states, int page, int count, int? urn);
	}
}