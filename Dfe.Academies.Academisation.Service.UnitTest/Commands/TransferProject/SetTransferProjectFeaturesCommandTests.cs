using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferProjectFeaturesCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var urn = 123;
			var typeOfTransfer = "TransferTypeTest";
			var whoInitiated = "TestInitiator";
			var isCompleted = true;

			// Act
			var command = new SetTransferProjectFeaturesCommand
			{
				Urn = urn,
				TypeOfTransfer = typeOfTransfer,
				WhoInitiatedTheTransfer = whoInitiated,
				IsCompleted = isCompleted
			};

			// Assert
			Assert.Equal(urn, command.Urn);
			Assert.Equal(typeOfTransfer, command.TypeOfTransfer);
			Assert.Equal(whoInitiated, command.WhoInitiatedTheTransfer);
			Assert.Equal(isCompleted, command.IsCompleted);
		}
	}
}
