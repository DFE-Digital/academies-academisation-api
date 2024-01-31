namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public sealed class FormAMatProjectServiceModel
	{
		public FormAMatProjectServiceModel(int id, string proposedTrustName, string applicationReference)
		{
			Id = id;
			ProposedTrustName = proposedTrustName;
			ApplicationReference = applicationReference;
		}

		public int Id { get; init; }

		public string ProposedTrustName { get; init; }
		public string ApplicationReference { get; init; }

		public List<ConversionProjectServiceModel> projects { get; init; }

	}
}
