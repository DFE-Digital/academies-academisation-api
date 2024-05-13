namespace Dfe.Academies.Academisation.IService.ServiceModels.TransferProject
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
