﻿using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public interface IConversionProjectRepository : IRepository<Project>, IGenericRepository<Project>
{
	Task<(IEnumerable<IProject>, int)> SearchProjects(
	IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, int page, int count, int? urn, IEnumerable<string>? regions = default, IEnumerable<string>? applicationReferences = default);

	Task<IProject?> GetConversionProject(int id);

	Task<ProjectFilterParameters> GetFilterParameters();
	Task<(IEnumerable<IProject> projects, int totalCount)> SearchProjectsV2(
	IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers,  IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count);
	Task<IEnumerable<IProject>> GetConversionProjectsThatRequireFormAMatCreation(CancellationToken cancellationToken);
	Task<(IEnumerable<IProject> projects, int totalCount)> SearchFormAMatProjects(
	IEnumerable<string>? states, string? title, IEnumerable<string>? deliveryOfficers, IEnumerable<string>? regions, IEnumerable<string>? localAuthorities, IEnumerable<string>? advisoryBoardDates, int page, int count);
}
