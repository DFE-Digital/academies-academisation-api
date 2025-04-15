using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.TransferProjectAggregate
{
	public class TransferProjectTests
	{
		private readonly string _outgoingTrustUkprn = "12345678";
		private readonly string _outgoingTrusName = "_outgoingTrusName";
		private readonly static string _incomingTrustUkprn = "23456789";
		private readonly static string _incomingTrustName = "_incomingTrustName";

		private readonly List<TransferringAcademy> _academies = new List<TransferringAcademy>() {
			new TransferringAcademy(_incomingTrustUkprn, _incomingTrustName, "academy1", "region", "local authority"),
			new TransferringAcademy(_incomingTrustUkprn, _incomingTrustName, "academy2", "region", "local authority") };

		private readonly bool _isFormAMat = true;
		private readonly DateTime _createdOn = DateTime.Now;
		public TransferProjectTests()
		{

		}

		[Fact]
		public void GenerateUrn_WithMultipleAcademies_GivesAMATBasedReference()
		{
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrustName = "incomingTrustName";
			List<TransferringAcademy> academies = new List<TransferringAcademy>() { new TransferringAcademy(incomingTrustUkprn, incomingTrustName, "22221111", "region", "local authority"), new TransferringAcademy(incomingTrustUkprn, incomingTrustName, "33331111", "region", "local authority") };
			bool isFormAMat = true;
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				outgoingTrustName,
				academies,
				isFormAMat,
				createdOn);

			// Act
			result.GenerateUrn(10101020);

			// Assert
			result.ProjectReference.Should().Be("MAT-10101020");
		}

		[Fact]
		public void GenerateUrn_WithOneAcademy_GivesASATBasedReference()
		{
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrustName = "incomingTrustName";
			var academies = new List<TransferringAcademy>() { new(incomingTrustUkprn, incomingTrustName, "22221111", "region", "local authority") };

			bool isFormAMat = true;
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				outgoingTrustName,
				academies,
				isFormAMat,
				createdOn);

			// Act
			result.GenerateUrn(10101020);

			// Assert
			result.ProjectReference.Should().Be("SAT-10101020");
		}

		[Fact]
		public void CreateTransferProject_WithValidParameters_CreatesTransferProject()
		{
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrustName = "incomingTrustName";
			var academies = new List<TransferringAcademy>() { new (incomingTrustUkprn, incomingTrustName, "22221111", "region", "local authority"), new TransferringAcademy(incomingTrustUkprn, incomingTrustName, "33331111", "region", "local authority") };
			bool isFormAMat = false;
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				outgoingTrustName,
				academies,
				isFormAMat,
				createdOn);

			// Assert
			result.OutgoingTrustUkprn.Should().Be(outgoingTrustUkprn);
			result.CreatedOn.Should().Be(createdOn);
			result.TransferringAcademies.Count.Should().Be(2);
			result.TransferringAcademies.SingleOrDefault(x => x.IncomingTrustUkprn == incomingTrustUkprn && x.OutgoingAcademyUkprn == "22221111").Should().NotBeNull();
			result.TransferringAcademies.SingleOrDefault(x => x.IncomingTrustUkprn == incomingTrustUkprn && x.OutgoingAcademyUkprn == "33331111").Should().NotBeNull();
		}

		[Fact]
		public void CreateTransferProject_WithNullOutgoingTrustUkprn_ThrowsArgumentNullException()
		{
			// Arrange      
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrustName = "incomingTrustName";
			var academies = new List<TransferringAcademy>() { new (incomingTrustUkprn, incomingTrustName, "22221111", "region", "local authority"), new TransferringAcademy(incomingTrustUkprn, incomingTrustName, "33331111", "region", "local authority") };
			bool isFormAMat = true;
			DateTime createdOn = DateTime.Now;

			// Act
			Assert.Throws<ArgumentNullException>(() => TransferProject.Create(
				null!,
				outgoingTrustName,
				academies,
				isFormAMat,
				createdOn));
		}

		[Fact]
		public void SetTransferProjectRationale_WithValidParameters_SetsCorrectProperties()
		{
			var fixture = new Fixture();

			// Arrange      
			TransferProject result = CreateValidTransferProject();
			var rationaleSectionIsCompleted = fixture.Create<bool>();
			var projectRationale = fixture.Create<string>();
			var trustSponsorRationale = fixture.Create<string>();

			//Act
			result.SetRationale(projectRationale, trustSponsorRationale, rationaleSectionIsCompleted);

			//Assert
			result.RationaleSectionIsCompleted.Should().Be(rationaleSectionIsCompleted);
			result.ProjectRationale.Should().Be(projectRationale);
			result.TrustSponsorRationale.Should().Be(trustSponsorRationale);
		}
		[Fact]
		public void SetTransferProjectTrustInformationAndProjectDates_WithValidParameters_SetsCorrectProperties()
		{
			Fixture fixture = new Fixture();

			// Arrange      
			TransferProject result = CreateValidTransferProject();
			var recommendation = fixture.Create<string>();
			var author = fixture.Create<string>();

			//Act
			result.SetGeneralInformation(recommendation, author);

			//Assert
			result.Recommendation.Should().Be(recommendation);
			result.Author.Should().Be(author);
		}
		[Fact]
		public void AssignTransferProjectUser_WithValidParameters_SetsCorrectProperties()
		{
			Fixture fixture = new Fixture();

			// Arrange      
			TransferProject result = CreateValidTransferProject();
			var userId = fixture.Create<Guid>();
			var userEmail = fixture.Create<string>();
			var userFullName = fixture.Create<string>();

			//Act
			result.AssignUser(userId, userEmail, userFullName);

			//Assert
			result.AssignedUserId.Should().Be(userId);
			result.AssignedUserEmailAddress.Should().Be(userEmail);
			result.AssignedUserFullName.Should().Be(userFullName);
		}

		[Fact]
		public void SetTransferProjectBenefits_WithValidParameters_SetsCorrectProperties()
		{
			Fixture fixture = new Fixture();

			// Arrange      
			TransferProject result = CreateValidTransferProject();
			var anyRisks = fixture.Create<bool>();
			bool? equalitiesImpactAssessmentConsidered = fixture.Create<bool>();
			List<string> selectedBenefits = fixture.Create<List<string>>();
			string? otherBenefitValue = fixture.Create<string>();
			bool? highProfileShouldBeConsidered = fixture.Create<bool>();
			string? highProfileFurtherSpecification = fixture.Create<string>();
			bool? complexLandAndBuildingShouldBeConsidered = fixture.Create<bool>();
			string? complexLandAndBuildingFurtherSpecification = fixture.Create<string>();
			bool? financeAndDebtShouldBeConsidered = fixture.Create<bool>();
			string? financeAndDebtFurtherSpecification = fixture.Create<string>();
			bool? otherRisksShouldBeConsidered = fixture.Create<bool>();
			string? otherRisksFurtherSpecification = fixture.Create<string>();
			bool? isCompleted = fixture.Create<bool>();

			//Act
			result.SetBenefitsAndRisks(
				anyRisks,
				equalitiesImpactAssessmentConsidered,
				selectedBenefits,
				otherBenefitValue,
				highProfileShouldBeConsidered,
				highProfileFurtherSpecification,
				complexLandAndBuildingShouldBeConsidered,
				complexLandAndBuildingFurtherSpecification,
				financeAndDebtShouldBeConsidered,
				financeAndDebtFurtherSpecification,
				otherRisksShouldBeConsidered,
				otherRisksFurtherSpecification,
				isCompleted);

			//Assert
			result.AnyRisks.Should().Be(anyRisks);
			result.EqualitiesImpactAssessmentConsidered.Should().Be(equalitiesImpactAssessmentConsidered);
			result.IntendedTransferBenefits.All(x => selectedBenefits.Contains(x.SelectedBenefit)).Should().BeTrue();
			result.OtherBenefitValue.Should().Be(otherBenefitValue);
			result.HighProfileShouldBeConsidered.Should().Be(highProfileShouldBeConsidered);
			result.HighProfileFurtherSpecification.Should().Be(highProfileFurtherSpecification);
			result.ComplexLandAndBuildingShouldBeConsidered.Should().Be(complexLandAndBuildingShouldBeConsidered);
			result.ComplexLandAndBuildingFurtherSpecification.Should().Be(complexLandAndBuildingFurtherSpecification);
			result.FinanceAndDebtShouldBeConsidered.Should().Be(financeAndDebtShouldBeConsidered);
			result.FinanceAndDebtFurtherSpecification.Should().Be(financeAndDebtFurtherSpecification);
			result.OtherRisksShouldBeConsidered.Should().Be(otherRisksShouldBeConsidered);
			result.OtherRisksFurtherSpecification.Should().Be(otherRisksFurtherSpecification);
			result.BenefitsSectionIsCompleted.Should().Be(isCompleted);
		}

		private static TransferProject CreateValidTransferProject()
		{
			string outgoingTrustUkprn = "11112222";
			string outgoingTrustName = "outgoingTrustName";
			string incomingTrustUkprn = "11110000";
			string incomingTrustName = "incomingTrustName";
			List<TransferringAcademy> academies = new List<TransferringAcademy>() { new TransferringAcademy(incomingTrustUkprn, incomingTrustName, "22221111", "region", "local authority"), new TransferringAcademy(incomingTrustUkprn, incomingTrustName, "33331111", "region", "local authority") };
			bool isFormAMat = true;
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				outgoingTrustName,
				academies,
				isFormAMat,
				createdOn);
			return result;
		}

		[Theory]
		[ClassData(typeof(CreationArgumentExceptionTestData))]
		public void CreateTransferProject_WithTestData_ThrowsArgumentExceptions(string outgoingTrustUkprn, string outgoingTrustName, List<TransferringAcademy> academies, bool? isFormAMat, DateTime createdOn, Type exType)
		{
			// Arrange    
			// Act
			if (exType == typeof(ArgumentException))
			{
				Assert.Throws<ArgumentException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   outgoingTrustName,
				   academies,
				   isFormAMat,
				   createdOn));
			}

			if (exType == typeof(ArgumentNullException))
			{
				Assert.Throws<ArgumentNullException>(() => TransferProject.Create(
				  outgoingTrustUkprn,
				   outgoingTrustName,
				   academies,
				   isFormAMat,
				   createdOn));
			}

			if (exType == typeof(ArgumentOutOfRangeException))
			{
				Assert.Throws<ArgumentOutOfRangeException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   outgoingTrustName,
				   academies,
				   isFormAMat,
				   createdOn));
			}
		}

		[Theory]
		[InlineData("Test Initiation", "test specific reason", "Test Type", true)]
		[InlineData("Another Initiation", "Another test specific reason", "Another Type", false)]
		public void SetFeatures_WithValidParameters_SetsPropertiesCorrectly(string whoInitiated, string specficReasons, string transferType, bool isCompleted)
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, _isFormAMat, _createdOn);
			var reasons = new List<string>() { specficReasons };
			// Act
			transferProject.SetFeatures(whoInitiated, reasons, transferType, isCompleted);

			// Assert
			transferProject.WhoInitiatedTheTransfer.Should().Be(whoInitiated);
			transferProject.TypeOfTransfer.Should().Be(transferType);
			transferProject.FeatureSectionIsCompleted.Should().Be(isCompleted);
		}
		[Theory]
		[InlineData("Test outgoing trust resolution", "Test incoming trust resolution", "diocese", true)]
		[InlineData("Test 123", "Test 345", "diocese 123", false)]
		public void SetLegalRequirements_WithValidParameters_SetsPropertiesCorrectly(string outgoingTrustResolution, string incomingTrustAgreement, string diocesanConsent, bool isCompleted)
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, _isFormAMat, _createdOn);

			// Act
			transferProject.SetLegalRequirements(outgoingTrustResolution, incomingTrustAgreement, diocesanConsent, isCompleted);

			// Assert
			transferProject.OutgoingTrustConsent.Should().Be(outgoingTrustResolution);
			transferProject.IncomingTrustAgreement.Should().Be(incomingTrustAgreement);
			transferProject.DiocesanConsent.Should().Be(diocesanConsent);
			transferProject.LegalRequirementsSectionIsCompleted.Should().Be(isCompleted);
		}
		[Fact]
		public void SetTransferDates_WithValidParameters_SetsPropertiesCorrectly()
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, _isFormAMat, _createdOn);
			var advisoryBoardDate = DateTime.UtcNow;
			var previousAdvisoryBoardDate = DateTime.UtcNow;
			var expectedDateForTransfer = DateTime.UtcNow.AddMonths(1);
			// Act
			transferProject.SetTransferDates(advisoryBoardDate, previousAdvisoryBoardDate, expectedDateForTransfer, true);

			// Assert
			transferProject.HtbDate.Should().Be(advisoryBoardDate);
			transferProject.TargetDateForTransfer.Should().Be(expectedDateForTransfer);
		}

		[Theory]
		[InlineData("Withdrwn")]
		[InlineData("Declined")]
		public void SetStatus_WithValidParameters_SetsPropertiesCorrectly(string status)
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, _isFormAMat, _createdOn);

			// Act
			transferProject.SetStatus(status);

			// Assert
			transferProject.Status.Should().Be(status);
		}

		[Theory]
		[InlineData("Likely", "Likely reason", true)]
		[InlineData("Unlikely", "", true)]
		[InlineData("Some impact", "Some impact reason", true)]
		public void SetPublicEqualityDuty_WithValidParameters_SetsPropertiesCorrectly(string publicEqualityDutyImpact, string publicEqualityDutyReduceImpactReason, bool publicEqualityDutySectionComplete)
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _outgoingTrusName, _academies, _isFormAMat, _createdOn);

			// Act
			transferProject.SetPublicEqualityDuty(publicEqualityDutyImpact, publicEqualityDutyReduceImpactReason, publicEqualityDutySectionComplete);

			// Assert
			transferProject.PublicEqualityDutyImpact.Should().Be(publicEqualityDutyImpact);
			transferProject.PublicEqualityDutyReduceImpactReason.Should().Be(publicEqualityDutyReduceImpactReason);
			transferProject.PublicEqualityDutySectionComplete.Should().Be(publicEqualityDutySectionComplete);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_ValidData_SetsPFIScheme()
		{
			// Arrange			
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act
			transferProject.SetTransferringAcademyGeneralInformation("academy1", "No", "Details", "12", "Details", "20.5", "Distance details", "Yes", "10");

			// Assert
			Assert.Equal("No", actual: transferProject.TransferringAcademies.FirstOrDefault()!.PFIScheme);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_ValidData_SetsDistanceToTrust()
		{
			// Arrange			
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act
			transferProject.SetTransferringAcademyGeneralInformation("academy1", "No", "Details", "12", "Distance Details", "No", "Yes", "Independant", "100");

			// Assert
			Assert.Equal("12", actual: transferProject.TransferringAcademies.FirstOrDefault()!.DistanceFromAcademyToTrustHq);
			Assert.Equal("Distance Details", actual: transferProject.TransferringAcademies.FirstOrDefault()!.DistanceFromAcademyToTrustHqDetails);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_ValidData_SetsViabilityIssues()
		{
			// Arrange			
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act
			transferProject.SetTransferringAcademyGeneralInformation("academy1", "No", "Details", "12", "Distance Details", "No", "Yes", "Independant", "100");

			// Assert
			Assert.Equal("No", actual: transferProject.TransferringAcademies.FirstOrDefault()!.ViabilityIssues);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_ValidData_SetsFinancialDeficit()
		{
			// Arrange			
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act
			transferProject.SetTransferringAcademyGeneralInformation("academy1", "No", "Details", "12", "Distance Details", "No", "Yes", "Independant", "100");

			// Assert
			Assert.Equal("Yes", actual: transferProject.TransferringAcademies.FirstOrDefault()!.FinancialDeficit);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_ValidData_SetsMPNameAndParty()
		{
			// Arrange			
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act
			transferProject.SetTransferringAcademyGeneralInformation("academy1", "No", "Details", "12", "Distance Details", "No", "Yes", "Independant", "100");

			// Assert
			Assert.Equal("Independant", actual: transferProject.TransferringAcademies.FirstOrDefault()!.MPNameAndParty);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_ValidData_SetsPublishedAdmissionNumber()
		{
			// Arrange			
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act
			transferProject.SetTransferringAcademyGeneralInformation("academy1", "No", "Details", "12", "Distance Details", "No", "Yes", "Independant", "100");

			// Assert
			Assert.Equal("100", actual: transferProject.TransferringAcademies.FirstOrDefault()!.PublishedAdmissionNumber);
		}

		[Fact]
		public void SetTransferringAcademyGeneralInformation_AcademyNotFound_ThrowsException()
		{
			// Arrange
			var transferProject = TransferProject.Create("12345678", "Outgoing Trust", _academies, false, DateTime.Now);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => transferProject.SetTransferringAcademyGeneralInformation("12345678", "No", "Details", "12", "Details", "20.5", "Distance details", "Yes", "10"));
		}
		public class CreationArgumentExceptionTestData : IEnumerable<object[]>
		{
			private List<TransferringAcademy> _transferringAcademies = new List<TransferringAcademy>() {
				new TransferringAcademy("11110000", "in trust", "22221111", "", ""),
				new TransferringAcademy("11110000", "in trust", "33331111", "", ""),
			};

			public IEnumerator<object[]> GetEnumerator()
			{
				yield return new object[] { null!, "out trust", _transferringAcademies, false, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { string.Empty, "out trust", _transferringAcademies, false, DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", "out trust", null!, false, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { "11112222", "out trust", new List<TransferringAcademy>(), false, DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", "out trust", _transferringAcademies, false, DateTime.MinValue, typeof(ArgumentOutOfRangeException) };
				yield return new object[] { "11112222", "out trust", _transferringAcademies, false, DateTime.MaxValue, typeof(ArgumentOutOfRangeException) };
				yield return new object[] { "11112222", "out trust", _transferringAcademies, false, null!, typeof(ArgumentOutOfRangeException) };
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
