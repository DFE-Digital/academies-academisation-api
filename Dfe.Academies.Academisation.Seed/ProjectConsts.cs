namespace Dfe.Academies.Academisation.Seed
{
	public static class ProjectConsts
	{
		public static string[] Statuses { get; } =
		{
			"Active", "Pre advisory board", "APPROVED WITH CONDITIONS", "Declined", "Deferred", "Approved",
			"Converter Pre-AO (C)",
		};

		public static string[] Regions { get; } =
		{
			"East Midlands", "East of England", "London", "North East", "North West", "South West", "South East",
			"West Midlands", "Yorkshire and the Humber",
		};
	}
}
