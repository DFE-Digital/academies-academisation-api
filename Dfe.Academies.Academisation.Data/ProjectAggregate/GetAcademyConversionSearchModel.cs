﻿namespace Dfe.Academies.Academisation.Data.ProjectAggregate
{
	public class GetAcademyConversionSearchModel
	{
		public GetAcademyConversionSearchModel(int page, int count, string? titleFilter,
			IEnumerable<string>? deliveryOfficerQueryString, IEnumerable<string>? regionQueryString,
			IEnumerable<string>? statusQueryString, IEnumerable<string>? applicationReferences, 
			IEnumerable<string>? localAuthoritiesQueryString, IEnumerable<string>? advisoryBoardDatesQueryString)
		{
			Page = page;
			Count = count;
			TitleFilter = titleFilter;
			DeliveryOfficerQueryString = deliveryOfficerQueryString;
			RegionQueryString = regionQueryString;
			StatusQueryString = statusQueryString;
			ApplicationReferences = applicationReferences;
			LocalAuthoritiesQueryString = localAuthoritiesQueryString;
			AdvisoryBoardDatesQueryString = advisoryBoardDatesQueryString;
		}

		public int Page { get; set; }
		public int Count { get; set; }
		public string? TitleFilter { get; set; }
		public IEnumerable<string>? DeliveryOfficerQueryString { get; set; }
		public IEnumerable<string>? RegionQueryString { get; set; }
		public IEnumerable<string>? StatusQueryString { get; set; }
		public IEnumerable<string>? ApplicationReferences { get; set; }
		public IEnumerable<string>? LocalAuthoritiesQueryString { get; }
		public IEnumerable<string>? AdvisoryBoardDatesQueryString { get; }
	}
}
