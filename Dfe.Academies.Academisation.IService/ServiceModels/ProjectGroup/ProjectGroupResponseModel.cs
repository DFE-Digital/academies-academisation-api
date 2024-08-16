using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.IService.ServiceModels.ProjectGroup
{
	public class ProjectGroupResponseModel(int id, string referenceNumber, string trustReferenceNumber, string trustName, string trustUkprn, User assignedUser, List<ConversionProjectServiceModel> projects)
	{
		public int Id { get; init; } = id;
		public string TrustReferenceNumber { get; init; } = trustReferenceNumber;

		public string TrustName { get; init ; } = trustName;
		public string TrustUkprn{ get; init ; } = trustUkprn;

		public string? ReferenceNumber { get; init; } = referenceNumber;
		public User AssignedUser { get; init; } = assignedUser;

		public List<ConversionProjectServiceModel> Projects { get; init; } = projects;

		public List<AcademyTransferProjectResponse>? Transfers { get; set; }
	}
}
