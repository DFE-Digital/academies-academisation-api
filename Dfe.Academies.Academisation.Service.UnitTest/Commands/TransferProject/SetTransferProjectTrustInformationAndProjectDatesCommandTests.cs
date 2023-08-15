using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject;

public class SetTransferProjectTrustInformationAndProjectDatesCommandTests
{
	[Fact]
	public void CommandProperties_AreSetCorrectly()
	{
		// Arrange
		var id = 123;
		var recommendation = "Recommendation";
		var author = "Author";

		// Act
		var command = new SetTransferProjectTrustInformationAndProjectDatesCommand
		{
			Id = id,
			Recommendation = recommendation,
			Author = author
		};

		// Assert
		Assert.Equal(id, command.Id);
		Assert.Equal(recommendation, command.Recommendation);
		Assert.Equal(author, command.Author);
	}
}
