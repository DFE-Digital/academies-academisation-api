namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class TransferProjectSearchModel
	{
		public TransferProjectSearchModel(string? titleFilter)
		{
			TitleFilter = titleFilter;
		}

		public string? TitleFilter { get; set; }
	}
}
