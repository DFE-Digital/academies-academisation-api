namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
{
	public class PagedResultResponse<T>
    {
        public PagedResultResponse(IEnumerable<T> results = null, int totalCount = 0)
        {
            Results = results ?? Enumerable.Empty<T>();
            TotalCount = totalCount;
        }

        public IEnumerable<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}