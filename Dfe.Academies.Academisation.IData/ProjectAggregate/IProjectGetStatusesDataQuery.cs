namespace Dfe.Academies.Academisation.IData.ProjectAggregate
{
	public interface IProjectStatusesDataQuery
	{
		Task<List<string?>> Execute();
	}
}
