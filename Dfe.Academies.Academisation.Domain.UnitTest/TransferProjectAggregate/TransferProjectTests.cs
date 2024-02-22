using AutoFixture;
using AutoMapper;
using Dfe.Academies.Academisation.Domain.TransferProjectAggregate;
using FluentAssertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.TransferProjectAggregate
{
	public class TransferProjectTests
	{
		private readonly string _outgoingTrustUkprn = "12345678";
		private readonly string _incomingTrustUkprn = "23456789";
		private readonly List<string> _academyUkprns = new() { "academy1", "academy2" };
		private readonly DateTime _createdOn = DateTime.Now;
		public TransferProjectTests()
		{

		}

		[Fact]
		public void GenerateUrn_WithMultipleAcademies_GivesAMATBasedReference()
		{
			// Arrange
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
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
			// Arrange      
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
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
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
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
			string outgoingTrustUkprn = "11112222";
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			Assert.Throws<ArgumentNullException>(() => TransferProject.Create(
			   null,
			   incomingTrustUkprn,
			   academyUkprns,
			   createdOn));
		}

		[Fact]
		public void SetTransferProjectRationale_WithValidParameters_SetsCorrectProperties()
		{
			Fixture fixture = new Fixture();

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
			string incomingTrustUkprn = "11110000";
			List<string> academyUkprns = new List<string>() { "22221111", "33331111" };
			DateTime createdOn = DateTime.Now;

			// Act
			var result = TransferProject.Create(
				outgoingTrustUkprn,
				incomingTrustUkprn,
				academyUkprns,
				createdOn);
			return result;
		}

		[Theory]
		[ClassData(typeof(CreationArgumentExceptionTestData))]
		public void CreateTransferProject_WithTestData_ThrowsArgumentExceptions(string outgoingTrustUkprn, string incomingTrustUkprn, List<string> academyUkprns, DateTime createdOn, Type exType)
		{
			// Arrange      
			dynamic exception;
			// Act
			if (exType == typeof(ArgumentException))
			{
				Assert.Throws<ArgumentException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   incomingTrustUkprn,
				   academyUkprns,
				   createdOn));
			}

			if (exType == typeof(ArgumentNullException))
			{
				Assert.Throws<ArgumentNullException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   incomingTrustUkprn,
				   academyUkprns,
				   createdOn));
			}

			if (exType == typeof(ArgumentOutOfRangeException))
			{
				Assert.Throws<ArgumentOutOfRangeException>(() => TransferProject.Create(
				   outgoingTrustUkprn,
				   incomingTrustUkprn,
				   academyUkprns,
				   createdOn));
			}
		}

		[Theory]
		[InlineData("Test Initiation", "test specific reason", "Test Type", true)]
		[InlineData("Another Initiation", "Another test specific reason", "Another Type", false)]
		public void SetFeatures_WithValidParameters_SetsPropertiesCorrectly(string whoInitiated, string specficReasons, string transferType, bool isCompleted)
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _incomingTrustUkprn, _academyUkprns, _createdOn);
			var reasons = new List<string>() { specficReasons };
			// Act
			transferProject.SetFeatures(whoInitiated, reasons, transferType, isCompleted);

			// Assert
			transferProject.WhoInitiatedTheTransfer.Should().Be(whoInitiated);
			transferProject.TypeOfTransfer.Should().Be(transferType);
			transferProject.FeatureSectionIsCompleted.Should().Be(isCompleted);
		}
		[Theory]
		[InlineData("Test outgoing trust resolution", "Test incoming trust resolution","diocese", true)]
		[InlineData("Test 123", "Test 345","diocese 123", false)]
		public void SetLegalRequirements_WithValidParameters_SetsPropertiesCorrectly(string outgoingTrustResolution, string incomingTrustAgreement,string diocesanConsent, bool isCompleted)
		{
			// Arrange
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _incomingTrustUkprn, _academyUkprns, _createdOn);

			// Act
			transferProject.SetLegalRequirements(outgoingTrustResolution, incomingTrustAgreement,diocesanConsent, isCompleted);

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
			var transferProject = TransferProject.Create(_outgoingTrustUkprn, _incomingTrustUkprn, _academyUkprns, _createdOn);
			var advisoryBoardDate = DateTime.UtcNow;
			var expectedDateForTransfer = DateTime.UtcNow.AddMonths(1);
			// Act
			transferProject.SetTransferDates(advisoryBoardDate, expectedDateForTransfer);

			// Assert
			transferProject.HtbDate.Should().Be(advisoryBoardDate);
			transferProject.TargetDateForTransfer.Should().Be(expectedDateForTransfer);
		}

		public class CreationArgumentExceptionTestData : IEnumerable<object[]>
		{
			public IEnumerator<object[]> GetEnumerator()
			{
				yield return new object[] { null, "11110000", new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { string.Empty, "11110000", new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", null, new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { "11112222", string.Empty, new List<string>() { "22221111", "33331111" }, DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", "11110000", null, DateTime.Now, typeof(ArgumentNullException) };
				yield return new object[] { "11112222", "11110000", new List<string>(), DateTime.Now, typeof(ArgumentException) };

				yield return new object[] { "11112222", "11110000", new List<string>() { "22221111", "33331111" }, DateTime.MinValue, typeof(ArgumentOutOfRangeException) };
				yield return new object[] { "11112222", "11110000", new List<string>() { "22221111", "33331111" }, DateTime.MaxValue, typeof(ArgumentOutOfRangeException) };
				yield return new object[] { "11112222", "11110000", new List<string>() { "22221111", "33331111" }, null, typeof(ArgumentOutOfRangeException) };
			}

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
