using System;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public class TestProjectModel
	{
		public TestProjectModel(int id, int urn)
		{
			Id = id;
			Urn = urn;
		}
		public int Id { get; init; }
		public int? Urn { get; init; }
	}
}
