namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class GetAcademyConversionSearchModel
	{
		public GetAcademyConversionSearchModel(int page, int count, string? titleFilter,
			IEnumerable<string>? deliveryOfficerQueryString, IEnumerable<int>? regionUrnsQueryString,
			IEnumerable<string>? statusQueryString)
		{
			Page = page;
			Count = count;
			TitleFilter = titleFilter;
			DeliveryOfficerQueryString = deliveryOfficerQueryString;
			RegionUrnsQueryString = regionUrnsQueryString;
			StatusQueryString = statusQueryString;
		}

		public int Page { get; set; }
		public int Count { get; set; }
		public string? TitleFilter { get; set; }
		public IEnumerable<string>? DeliveryOfficerQueryString { get; set; }
		public IEnumerable<int>? RegionUrnsQueryString { get; set; }
		public IEnumerable<string>? StatusQueryString { get; set; }
	}
}
