namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IProjectGetStatusesQuery
	{
		Task<List<string?>> Execute();
	}
}
