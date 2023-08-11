using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferProjectLegalRequirementsCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var id = 123;
			var incomingTrustAgreement = "trust 1 yes";
			var outgoingrustAgreement = "trust 2 yes";
			var diocesanConsent = "diocese yes";
			var isCompleted = true;

			// Act
			var command = new SetTransferProjectLegalRequirementsCommand
			{
				Id = id,
				IncomingTrustAgreement = incomingTrustAgreement,
				DiocesanConsent = diocesanConsent,
				IsCompleted = isCompleted,
				OutgoingTrustConsent = outgoingrustAgreement
			};

			// Assert
			Assert.Equal(id, command.Id);
			Assert.Equal(incomingTrustAgreement, command.IncomingTrustAgreement);
			Assert.Equal(outgoingrustAgreement, command.OutgoingTrustConsent);
			Assert.Equal(diocesanConsent, command.DiocesanConsent);
			Assert.Equal(isCompleted, command.IsCompleted);
		}
	}
}
