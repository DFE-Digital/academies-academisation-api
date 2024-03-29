﻿using Dfe.Academies.Academisation.Service.Commands.Application;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject;

public class AssignTransferProjectUserCommandTests
{
	[Fact]
	public void CommandProperties_AreSetCorrectly()
	{
		// Arrange
		var urn = 123;
		var userId = new Guid();
		var userEmail = "Email";
		var userFullName = "Full Name";
		

		// Act
		var command = new AssignTransferProjectUserCommand
		{
			Urn = urn,
			UserId = userId,
			UserEmail = userEmail,
			UserFullName = userFullName
		};

		// Assert
		Assert.Equal(urn, command.Urn);
		Assert.Equal(userId, command.UserId);
		Assert.Equal(userEmail, command.UserEmail);
		Assert.Equal(userFullName, command.UserFullName);
	}
}
