using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
    public class SetExternalApplicationFormCommandTests
    {
        [Fact]
        public void CommandProperties_AreSetCorrectly()
        {
			// Arrange
			var Id = 100101;
			var ExternalApplicationFormUrl = "http://test";
			var ExternalApplicationFormSaved = true;

			// Act
			var command = new SetExternalApplicationFormCommand(Id, ExternalApplicationFormSaved, ExternalApplicationFormUrl);


			// Assert
			Assert.Equal(Id, command.Id);
			Assert.Equal(ExternalApplicationFormUrl, command.ExternalApplicationFormUrl);
			Assert.Equal(ExternalApplicationFormSaved, command.ExternalApplicationFormSaved);
		}
    }
}
