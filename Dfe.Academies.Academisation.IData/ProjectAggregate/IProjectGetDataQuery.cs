using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectGetDataQuery
	{
		Task<IProject?> Execute(int id);
	}
}