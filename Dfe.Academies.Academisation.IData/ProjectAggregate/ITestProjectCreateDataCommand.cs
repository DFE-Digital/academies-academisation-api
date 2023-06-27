using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate;

public interface ITestProjectCreateDataCommand
{
	Task<IProject> Execute();
}
