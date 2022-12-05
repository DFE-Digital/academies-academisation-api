namespace Dfe.Academies.Academisation.IData.Establishment
{
	public class Establishment
	{
		public Establishment()
		{
			Gor = new Region();
		}

		public string? LocalAuthorityName { get; set; }
		public Region Gor { get; set; }

		public class Region
		{
			public string? Name { get; set; }
		}
	}
}
