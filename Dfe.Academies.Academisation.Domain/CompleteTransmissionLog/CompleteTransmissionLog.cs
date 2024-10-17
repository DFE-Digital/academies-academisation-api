using Dfe.Academies.Academisation.Domain.SeedWork;

namespace Dfe.Academies.Academisation.Domain.CompleteTransmissionLog
{
	public class CompleteTransmissionLog : Entity, ICompleteTransmissionLog, IAggregateRoot
	{
		private CompleteTransmissionLog(bool isSuccess, string response, DateTime createdOn)
		{
			TransferProjectId = null;
			ConversionProjectId = null;

			IsSuccess = isSuccess;
			Response = response;
			CreatedOn = createdOn;
		}

		public int? TransferProjectId { get; private set;  }
		public int? TransferringAcademyId { get; private set; }
		public int? ConversionProjectId { get; private set;  }
		public Guid? CompleteProjectId { get; private set; }

		public bool IsSuccess { get; private set; }
		public string Response { get; private set; }

		public static CompleteTransmissionLog CreateConversionProjectLog(int projectId, Guid? completeProjectId, bool isSuccess, string response, DateTime createdOn)
		{
			return new CompleteTransmissionLog(isSuccess, response, createdOn) { ConversionProjectId = projectId, CompleteProjectId = completeProjectId };
		}
		public static CompleteTransmissionLog CreateTransferProjectLog(int projectId, int transferringAcdemyId, Guid? completeProjectId, bool isSuccess, string response, DateTime createdOn)
		{
			return new CompleteTransmissionLog(isSuccess, response, createdOn) { TransferProjectId = projectId, TransferringAcademyId = transferringAcdemyId, CompleteProjectId = completeProjectId };
		}
	}
}
