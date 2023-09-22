namespace Dfe.Academies.Academisation.IService.ServiceModels.Academies
{
	public class Establishment
	{
		public Establishment()
		{
			Gor = new Region();
		}

		public string? LocalAuthorityName { get; set; }
		public NameAndCodeResponse PhaseOfEducation { get; set; }
		public Region Gor { get; set; }

		public class Region
		{
			public string? Name { get; set; }
		}
		public class NameAndCodeResponse
		{
			public string Name { get; set; }
			public string Code { get; set; }
		}
	}
}
