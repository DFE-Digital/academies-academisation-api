using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Moq;

namespace Dfe.Academies.Academisation.Service.UnitTest.Mocks
{
	public record MockTransferAcademyRecord(string incomingTrustUkprn, string outgoingAcademyUkprn, string incomingTrustName);

	internal class MockTransferProject
	{
		public MockTransferProject(string outgoingTrustUkprn, string outgoingTrustName, List<MockTransferAcademyRecord> transferringAcademies)
		{
			OutgoingTrustUkprn = outgoingTrustUkprn;
			OutgoingTrustName = outgoingTrustName;

			foreach (var academy in transferringAcademies)
			{
				AddTransferringAcademy(academy);
			}
		}

		public string OutgoingTrustUkprn { get; set; }
		public string OutgoingTrustName { get; set; }
		public List<TransferringAcademy> TransferringAcademies { get; set; } = new();

		public void AddTransferringAcademy(MockTransferAcademyRecord mockTransferringAcademy)
		{
			var newTransferringAcademy = new TransferringAcademy(mockTransferringAcademy.incomingTrustUkprn, mockTransferringAcademy.outgoingAcademyUkprn);
			newTransferringAcademy.SetIncomingTrustName(mockTransferringAcademy.incomingTrustName);
			TransferringAcademies.Add(newTransferringAcademy);
		}

		public Mock<ITransferProject> CreateMock()
		{
			var mockTransferProject = new Mock<ITransferProject>();
			mockTransferProject.SetupGet(transferProject => transferProject.OutgoingTrustUkprn).Returns(OutgoingTrustUkprn);
			mockTransferProject.SetupGet(transferProject => transferProject.OutgoingTrustName).Returns(OutgoingTrustName);
			mockTransferProject.SetupGet(transferProject => transferProject.TransferringAcademies).Returns(TransferringAcademies);

			return mockTransferProject;
		}
	}
}
