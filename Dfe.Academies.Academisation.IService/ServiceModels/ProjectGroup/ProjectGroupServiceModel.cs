namespace Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup
{
	public class ProjectGroupServiceModel(string trustUrn)
	{
		public string TrustUrn { get; private set; } = trustUrn;

		public List<int> ConversionsUrns { get; set; } = new();
	}
}
