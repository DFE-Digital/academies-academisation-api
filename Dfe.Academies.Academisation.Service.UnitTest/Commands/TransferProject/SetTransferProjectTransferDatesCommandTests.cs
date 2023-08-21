using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferProjectTransferDatesCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var urn = 123;
			var advisoryBoardDate = DateTime.UtcNow;
			var targetDateForTransfer = DateTime.UtcNow.AddMonths(1);
			

			// Act
			var command = new SetTransferProjectTransferDatesCommand
			{
				Urn = urn,
				HtbDate = advisoryBoardDate,
				TargetDateForTransfer = targetDateForTransfer
			};

			// Assert
			Assert.Equal(urn, command.Urn);
			Assert.Equal(advisoryBoardDate, command.HtbDate);
			Assert.Equal(targetDateForTransfer, command.TargetDateForTransfer);
			
		}
	}
}
