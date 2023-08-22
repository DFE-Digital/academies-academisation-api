using Xunit;
using FluentAssertions;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.UnitTest.TransferProjectAggregate
{
	public class TransferringAcademyTests
	{
		[Fact]
		public void Constructor_InitializesCorrectly()
		{
			// Arrange
			var incomingTrustUkprn = "12345";
			var outgoingAcademyUkprn = "67890";

			// Act
			var academy = new TransferringAcademy(incomingTrustUkprn, outgoingAcademyUkprn);

			// Assert
			academy.IncomingTrustUkprn.Should().Be(incomingTrustUkprn);
			academy.OutgoingAcademyUkprn.Should().Be(outgoingAcademyUkprn);
		}

		[Fact]
		public void SetSchoolAdditionalData_SetsPropertiesCorrectly()
		{
			// Arrange
			var academy = new TransferringAcademy("12345", "67890");
			var ofstedReport = "Ofsted Report Data";
			var pupilNumbers = "Pupil Numbers Data";
			var keyStage2Performance = "Key Stage 2 Data";
			var keyStage4Performance = "Key Stage 4 Data";
			var keyStage5Performance = "Key Stage 5 Data";

			// Act
			academy.SetSchoolAdditionalData(ofstedReport, pupilNumbers, keyStage2Performance, keyStage4Performance, keyStage5Performance);

			// Assert
			academy.LatestOfstedReportAdditionalInformation.Should().Be(ofstedReport);
			academy.PupilNumbersAdditionalInformation.Should().Be(pupilNumbers);
			academy.KeyStage2PerformanceAdditionalInformation.Should().Be(keyStage2Performance);
			academy.KeyStage4PerformanceAdditionalInformation.Should().Be(keyStage4Performance);
			academy.KeyStage5PerformanceAdditionalInformation.Should().Be(keyStage5Performance);
		}
	}
}
