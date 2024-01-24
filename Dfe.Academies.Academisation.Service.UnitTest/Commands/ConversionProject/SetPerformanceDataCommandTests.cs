using Dfe.Academies.Academisation.Service.Commands.ConversionProject;
using Moq;
using System;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject
{
	public class SetPerformanceDataCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var Id = 100101;
			string? keyStage2PerformanceAdditionalInformation = "Test 1";
			string? keyStage4PerformanceAdditionalInformation = "Test 2";
			string? keyStage5PerformanceAdditionalInformation = "Test 3";
			string? educationalAttendanceAdditionalInformation = "Test 4";

			// Act
			var command = new SetPerformanceDataCommand(Id, keyStage2PerformanceAdditionalInformation, keyStage4PerformanceAdditionalInformation, keyStage5PerformanceAdditionalInformation, educationalAttendanceAdditionalInformation);


			// Assert
			Assert.Equal(Id, command.Id);
			Assert.Equal(keyStage2PerformanceAdditionalInformation, command.KeyStage2PerformanceAdditionalInformation);
			Assert.Equal(keyStage4PerformanceAdditionalInformation, command.KeyStage4PerformanceAdditionalInformation);
			Assert.Equal(keyStage5PerformanceAdditionalInformation, command.KeyStage5PerformanceAdditionalInformation);
			Assert.Equal(educationalAttendanceAdditionalInformation, command.EducationalAttendanceAdditionalInformation);
		}
	}
}
