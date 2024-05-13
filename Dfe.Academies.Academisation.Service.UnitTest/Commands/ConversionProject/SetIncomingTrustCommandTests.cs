using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SetCommands;
using Moq;
using System;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
	public class SetIncomingTrustCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var Id = 100101;
			string trustReferrenceNumber = "Test 1";
			string trustName = "Test 2";

			// Act
			var command = new SetIncomingTrustCommand(Id, trustReferrenceNumber, trustName);


			// Assert
			Assert.Equal(Id, command.Id);
			Assert.Equal(trustReferrenceNumber, command.TrustReferrenceNumber);
			Assert.Equal(trustName, command.TrustName);
		}
	}
}
