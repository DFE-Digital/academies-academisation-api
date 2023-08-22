using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject;

public class SetTransferProjectTrustInformationAndProjectDatesCommandTests
{
	[Fact]
	public void CommandProperties_AreSetCorrectly()
	{
		// Arrange
		var urn = 123;
		var recommendation = "Recommendation";
		var author = "Author";

		// Act
		var command = new SetTransferProjectTrustInformationAndProjectDatesCommand
		{
			Urn = urn,
			Recommendation = recommendation,
			Author = author
		};

		// Assert
		Assert.Equal(urn, command.Urn);
		Assert.Equal(recommendation, command.Recommendation);
		Assert.Equal(author, command.Author);
	}
}
