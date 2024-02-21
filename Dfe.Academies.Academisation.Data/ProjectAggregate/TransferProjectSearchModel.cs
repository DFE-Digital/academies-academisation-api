namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class TransferProjectSearchModel
	{
		public TransferProjectSearchModel(int page, int count, string? titleFilter)
		{
			Page = page;
			Count = count;
			TitleFilter = titleFilter;
		}

		public int Page { get; set; }
		public int Count { get; set; }
		public string? TitleFilter { get; set; }
	}
}
