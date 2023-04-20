namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class GetAcademyConversionSearchModel
	{
		public GetAcademyConversionSearchModel(int page, int count, string? titleFilter,
			IEnumerable<string>? deliveryOfficerQueryString, IEnumerable<string>? regionQueryString,
			IEnumerable<string>? statusQueryString, IEnumerable<string>? applicationReferences)
		{
			Page = page;
			Count = count;
			TitleFilter = titleFilter;
			DeliveryOfficerQueryString = deliveryOfficerQueryString;
			RegionQueryString = regionQueryString;
			StatusQueryString = statusQueryString;
			ApplicationReferences = applicationReferences;
		}

		public int Page { get; set; }
		public int Count { get; set; }
		public string? TitleFilter { get; set; }
		public IEnumerable<string>? DeliveryOfficerQueryString { get; set; }
		public IEnumerable<string>? RegionQueryString { get; set; }
		public IEnumerable<string>? StatusQueryString { get; set; }
		public IEnumerable<string>? ApplicationReferences { get; set; }
	}
}
