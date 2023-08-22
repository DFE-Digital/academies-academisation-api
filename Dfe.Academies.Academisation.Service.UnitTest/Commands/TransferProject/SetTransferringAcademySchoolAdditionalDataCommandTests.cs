using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferringAcademySchoolAdditionalDataCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var urn = 123;
			var latestOfsteadReportAdditionalInformation = "Ofsted";
			var pupilNumbersAdditionalInformation = "Pupil";
			var keyStage2PerformanceAdditionalInformation = "KS2";
			var keyStage4PerformanceAdditionalInformation = "KS4";
			var keyStage5PerformanceAdditionalInformation = "KS5";


			// Act
			var command = new SetTransferringAcademySchoolAdditionalDataCommand
			{
				Urn = urn,
				LatestOfstedReportAdditionalInformation = latestOfsteadReportAdditionalInformation,
				PupilNumbersAdditionalInformation = pupilNumbersAdditionalInformation,
				KeyStage2PerformanceAdditionalInformation = keyStage2PerformanceAdditionalInformation,
				KeyStage4PerformanceAdditionalInformation = keyStage4PerformanceAdditionalInformation,
				KeyStage5PerformanceAdditionalInformation = keyStage5PerformanceAdditionalInformation
			};

			// Assert
			Assert.Equal(urn, command.Urn);
			Assert.Equal(latestOfsteadReportAdditionalInformation, command.LatestOfstedReportAdditionalInformation);
			Assert.Equal(pupilNumbersAdditionalInformation, command.PupilNumbersAdditionalInformation);
			Assert.Equal(keyStage2PerformanceAdditionalInformation, command.KeyStage2PerformanceAdditionalInformation);
			Assert.Equal(keyStage4PerformanceAdditionalInformation, command.KeyStage4PerformanceAdditionalInformation);
			Assert.Equal(keyStage5PerformanceAdditionalInformation, command.KeyStage5PerformanceAdditionalInformation);
			

		}
	}
}
