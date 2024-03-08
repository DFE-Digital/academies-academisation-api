namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate
{
	public sealed class FormAMatProjectServiceModel
	{
		public FormAMatProjectServiceModel(int id, string proposedTrustName, string applicationReference, User assignedUser, string referenceNumber)
		{
			Id = id;
			ProposedTrustName = proposedTrustName;
			ApplicationReference = applicationReference;
			AssignedUser = assignedUser;
			ReferenceNumber = referenceNumber;
		}

		public int Id { get; init; }

		public string ProposedTrustName { get; init; }
		public string ApplicationReference { get; init; }
		public string ReferenceNumber { get; init; }
		public User AssignedUser { get; init; }


		public List<ConversionProjectServiceModel> projects { get; init; }

	}
}
