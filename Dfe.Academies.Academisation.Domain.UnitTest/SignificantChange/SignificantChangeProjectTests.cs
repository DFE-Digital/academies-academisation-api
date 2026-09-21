using System;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.SignificantChange
{
	public class SignificantChangeProjectTests
	{
		private readonly Fixture _fixture = new();

		[Fact]
		public void Constructor_ShouldSetPropertiesCorrectly()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var urn = _fixture.Create<int>();
			var tier = _fixture.Create<byte>();
			var trustName = _fixture.Create<string>();
			var trustUkprn = _fixture.Create<string>();
			var typeOfSignificantChange = _fixture.Create<string>();
			var schoolName = _fixture.Create<string>();
			var localAuthorityName = _fixture.Create<string>();
			var companiesHouseNumber = _fixture.Create<string>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				urn,
				tier,
				trustName,
				trustUkprn,
				typeOfSignificantChange,
				schoolName,
				localAuthorityName,
				companiesHouseNumber
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.Status.Should().Be(status);
			project.Urn.Should().Be(urn);
			project.Tier.Should().Be(tier);
			project.TrustName.Should().Be(trustName);
			project.TrustUkprn.Should().Be(trustUkprn);
			project.TypeOfSignificantChange.Should().Be(typeOfSignificantChange);
			project.SchoolName.Should().Be(schoolName);
			project.LocalAuthorityName.Should().Be(localAuthorityName);
			project.CompaniesHouseNumber.Should().Be(companiesHouseNumber);
			project.AssignedUserId.Should().BeNull();
			project.AssignedUserFullName.Should().BeNull();
			project.AssignedUserEmailAddress.Should().BeNull();
		}

		[Fact]
		public void AssignUser_ShouldSetUserProperties()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			var userId = _fixture.Create<Guid>();
			var userEmail = _fixture.Create<string>();
			var userFullName = _fixture.Create<string>();

			project.AssignUser(userId, userEmail, userFullName);

			project.AssignedUserId.Should().Be(userId);
			project.AssignedUserEmailAddress.Should().Be(userEmail);
			project.AssignedUserFullName.Should().Be(userFullName);


		}

		[Theory]
		[InlineData(SignificantChangeStatus.PreDecision)]
		[InlineData(SignificantChangeStatus.Approved)]
		[InlineData(SignificantChangeStatus.ApprovedWithConditions)]
		[InlineData(SignificantChangeStatus.Deferred)]
		[InlineData(SignificantChangeStatus.Declined)]
		[InlineData(SignificantChangeStatus.Withdrawn)]
		public void SetStatus_ShouldUpdateStatus(SignificantChangeStatus expectedStatus)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetStatus(expectedStatus);

			project.Status.Should().Be(expectedStatus);
		}

		[Fact]
		public void SetReadOnlyDate_ShouldSetReadOnlyDate()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);
			var project = SignificantChangeProject.Create(significantChangeProjectOptions, DateTime.UtcNow);

			var readOnlyDate = DateTime.UtcNow.AddDays(-1);

			project.SetReadOnlyDate(readOnlyDate);

			project.ReadOnlyDate.Should().Be(readOnlyDate);
		}

		[Fact]
		public void SetStakeholderConsultation_ShouldSetDetailsProperties()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderConsultation(false, "Trust has not consulted stakeholders yet");

			project.Details.TrustConsultedStakeholders.Should().BeFalse();
			project.Details.TrustConsultedStakeholdersNotConsultedReason.Should()
				.Be("Trust has not consulted stakeholders yet");
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{

			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenNotConsultedWithoutReason_ReturnsInProgress()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderConsultation(false, null);

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenConsulted_ReturnsCompleted()
		{
			var status = _fixture.Create<SignificantChangeStatus>();
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderConsultation(true, null);

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenNotConsultedWithReason_ReturnsCompleted()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderConsultation(false, "Consultation timeline does not allow this yet");

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetStakeholderConsultation_WhenNotConsulted_AndTierOne_MovesToTierTwo()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project =
				new SignificantChangeProject(SignificantChangeStatus.PreDecision, significantChangeProjectOptions);

			project.SetStakeholderConsultation(false, "No consultation carried out");

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetStakeholderConsultation_WhenTierMovedToTwo_DoesNotRevertToTierOne()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderConsultation(false, "No consultation carried out");
			project.SetStakeholderConsultation(true, null);

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetReligiousBodyConsultation_ShouldSetDetailsProperties()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(_fixture.Create<SignificantChangeStatus>(), significantChangeProjectOptions);

			project.SetReligiousBodyConsultation(false, "Trust has not consulted religious body yet");

			project.Details.TrustConsultedReligiousBody.Should().BeFalse();
			project.Details.TrustConsultedReligiousBodyNotConsultedReason.Should().Be("Trust has not consulted religious body yet");
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(_fixture.Create<SignificantChangeStatus>(), significantChangeProjectOptions);

			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenNotConsultedWithoutReason_ReturnsInProgress()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(_fixture.Create<SignificantChangeStatus>(), significantChangeProjectOptions);

			project.SetReligiousBodyConsultation(false, null);

			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenConsulted_ReturnsCompleted()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(_fixture.Create<SignificantChangeStatus>(), significantChangeProjectOptions);

			project.SetReligiousBodyConsultation(true, null);

			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenNotConsultedWithReason_ReturnsCompleted()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(_fixture.Create<SignificantChangeStatus>(), significantChangeProjectOptions);

			project.SetReligiousBodyConsultation(false, "Consultation timeline does not allow this yet");

			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetReligiousBodyConsultation_WhenNotConsulted_AndTierOne_MovesToTierTwo()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());

			var project = new SignificantChangeProject(SignificantChangeStatus.PreDecision, significantChangeProjectOptions);

			project.SetReligiousBodyConsultation(false, "No consultation carried out");

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetReligiousBodyConsultation_WhenTierMovedToTwo_DoesNotRevertToTierOne()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());

			var project = new SignificantChangeProject(SignificantChangeStatus.PreDecision, significantChangeProjectOptions);

			project.SetReligiousBodyConsultation(false, "No consultation carried out");
			project.SetReligiousBodyConsultation(true, null);

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetLocalAuthorityObjections_ShouldSetDetailsProperties()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: true,
				localAuthorityObjectionsFurtherInformation: "The local authority has raised objections about safeguarding",
				supportingEvidenceLink: "https://example.org/evidence");

			project.Details.LocalAuthorityRaisedObjections.Should().BeTrue();
			project.Details.LocalAuthorityObjectionsFurtherInformation.Should()
				.Be("The local authority has raised objections about safeguarding");
			project.Details.SupportingEvidenceLink.Should().Be("https://example.org/evidence");
		}

		[Fact]
		public void SetLocalAuthorityObjections_WhenNotRaised_ClearsFurtherInformation_AndSetsEvidenceDirectly()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: true,
				localAuthorityObjectionsFurtherInformation: "Initial objection details",
				supportingEvidenceLink: "https://example.org/evidence");

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: false,
				localAuthorityObjectionsFurtherInformation: "Should be cleared",
				supportingEvidenceLink: "https://example.org/cleared");

			project.Details.LocalAuthorityRaisedObjections.Should().BeFalse();
			project.Details.LocalAuthorityObjectionsFurtherInformation.Should().BeNull();
			project.Details.SupportingEvidenceLink.Should().Be("https://example.org/cleared");
		}

		[Fact]
		public void GetLocalAuthorityObjectionsTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.Details.GetLocalAuthorityObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetLocalAuthorityObjectionsTaskStatus_WhenOnlyEvidenceIsProvided_ReturnsInProgress()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: null,
				localAuthorityObjectionsFurtherInformation: null,
				supportingEvidenceLink: "https://example.org/evidence");

			project.Details.GetLocalAuthorityObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetLocalAuthorityObjectionsTaskStatus_WhenRaisedWithoutFurtherInformation_ReturnsInProgress()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: true,
				localAuthorityObjectionsFurtherInformation: null,
				supportingEvidenceLink: "https://example.org/evidence");

			project.Details.GetLocalAuthorityObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetLocalAuthorityObjectionsTaskStatus_WhenNotRaised_ReturnsCompleted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: false,
				localAuthorityObjectionsFurtherInformation: null,
				supportingEvidenceLink: null);

			project.Details.GetLocalAuthorityObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetLocalAuthorityObjectionsTaskStatus_WhenRaisedWithFurtherInformationAndNoEvidence_ReturnsCompleted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: true,
				localAuthorityObjectionsFurtherInformation: "Objection details",
				supportingEvidenceLink: null);

			project.Details.GetLocalAuthorityObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetLocalAuthorityObjections_WhenRaisedAndTierOne_MovesToTierTwo()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					1,
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetLocalAuthorityObjections(
				localAuthorityRaisedObjections: true,
				localAuthorityObjectionsFurtherInformation: "Objection details",
				supportingEvidenceLink: "https://example.org/evidence");

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetProjectDates_ShouldSetDates()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			var proposedDecisionDate = DateTime.UtcNow.AddDays(10);
			var proposedChangeDate = DateTime.UtcNow.AddDays(11);

			project.SetProjectDates(proposedDecisionDate, proposedChangeDate);

			project.Details.ProposedDecisionDate.Should().Be(proposedDecisionDate);
			project.Details.ProposedChangeDate.Should().Be(proposedChangeDate);
		}

		[Theory]
		[InlineData(null, null, SignificantChangeTaskStatus.NotStarted)]
		[InlineData("2024-07-01", null, SignificantChangeTaskStatus.InProgress)]
		[InlineData(null, "2024-07-01", SignificantChangeTaskStatus.InProgress)]
		[InlineData("2024-07-01", "2024-07-02", SignificantChangeTaskStatus.Completed)]
		public void GetProjectDates_ShouldHaveCorrectStatus(string? proposedDecisionDateString,
			string? proposedChangeDateString, SignificantChangeTaskStatus expectedTaskStatus)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			DateTime? proposedDecisionDate = string.IsNullOrEmpty(proposedDecisionDateString)
				? null
				: DateTime.Parse(proposedDecisionDateString);
			DateTime? proposedChangeDate = string.IsNullOrEmpty(proposedChangeDateString)
				? null
				: DateTime.Parse(proposedChangeDateString);

			project.Details.ProposedDecisionDate = proposedDecisionDate;
			project.Details.ProposedChangeDate = proposedChangeDate;

			project.Details.GetConfirmProjectDatesTaskStatus().Should().Be(expectedTaskStatus);
		}

		[Fact]
		public void GetEqualitiesTaskStatus_WhenNoValues_ReturnNotStarted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);


			project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void GetEqualitiesTaskStatus_WhenSetAssessmentHasBeenCompleted_ReturnInprogress(
			bool equalitiesImpactAssessmentCompleted)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);


			project.SetEqualitiesImpactAssessment(equalitiesImpactAssessmentCompleted, null, null);

			project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Theory]
		[InlineData(EqualitiesImpact.None)]
		[InlineData(EqualitiesImpact.PotentialImpacts)]
		public void GetEqualitiesTaskStatus_WhenImpactsHaveBeenSet_ReturnCompleted(EqualitiesImpact equalitiesImpact)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);


			project.SetEqualitiesImpactAssessment(true, equalitiesImpact, null);

			project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Theory]
		[InlineData(EqualitiesImpact.ImpactsIdentified, "", SignificantChangeTaskStatus.Completed)]
		[InlineData(EqualitiesImpact.ImpactsIdentified, null, SignificantChangeTaskStatus.Completed)]
		[InlineData(EqualitiesImpact.ImpactsIdentified, "Mitigation plan in place",
			SignificantChangeTaskStatus.Completed)]
		public void GetEqualitiesTaskStatus_WhenImpactsHaveBeenIdentified_ShouldReturnCorrectStatus(
			EqualitiesImpact impact, string? mitigation, SignificantChangeTaskStatus expectedStatus)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);


			project.SetEqualitiesImpactAssessment(true, impact, mitigation);

			project.Details.GetEqualitiesTaskStatus().Should().Be(expectedStatus);
		}

	[Fact]
		public void SetStakeholderObjections_ShouldSetDetailsProperties()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderObjections(SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided, "No further information supplied");

			project.Details.StakeholderObjections.Should().Be(SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided);
			project.Details.StakeholderObjectionsComment.Should().Be("No further information supplied");
		}

		[Fact]
		public void GetStakeholderObservationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{

			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.Details.GetStakeholderObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetStakeholderObservationsTaskStatus_When_YesNoFurtherInformationProvided_Without_Comment_ReturnsInProgress()
		{
			var status = _fixture.Create<SignificantChangeStatus>();

			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderObjections(SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided, null);

			project.Details.GetStakeholderObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetStakeholderObjectionsTaskStatus_When_YesAllObjectionsAddressed_ReturnsCompleted()
		{
			var status = _fixture.Create<SignificantChangeStatus>();
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderObjections(SignificantChangeStakeholderObjections.YesAllObjectionsAddressed, null);

			project.Details.GetStakeholderObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetStakeholderObjectionsTaskStatus_When_No_ReturnsCompleted()
		{
			var status = _fixture.Create<SignificantChangeStatus>();
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderObjections(SignificantChangeStakeholderObjections.No, null);

			project.Details.GetStakeholderObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetStakeholderObjectionsTaskStatus_When_YesNoFurtherInformationProvided_With_Comment_ReturnsCompleted()
		{
			var status = _fixture.Create<SignificantChangeStatus>();
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project = new SignificantChangeProject(status, significantChangeProjectOptions);

			project.SetStakeholderObjections(SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided, "awaiting stakeholder objection deets");

			project.Details.GetStakeholderObjectionsTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetStakeholderObjections_WhenYesNoFurtherInformationProvidedd_AndTierOne_MovesToTierTwo()
		{
			var significantChangeProjectOptions = new SignificantChangeProjectOptions(
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var project =
				new SignificantChangeProject(SignificantChangeStatus.PreDecision, significantChangeProjectOptions);

			project.SetStakeholderObjections(SignificantChangeStakeholderObjections.YesNoFurtherInformationProvided, "No further info");

			project.Tier.Should().Be(2);
		}

    
		[Fact]
		public void SetAdmissionVariationConsultation_ShouldSetDetailsProperties()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetAdmissionVariationConsultation(false, "No admission variation required");

			project.Details.ConsultationIncludeAdmissionVariation.Should().BeFalse();
			project.Details.ConsultationNoAdmissionVariationReason.Should().Be("No admission variation required");
		}


		[Fact]
		public void GetAdmissionVariationConsultationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.Details.GetAdmissionVariationConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetAdmissionVariationConsultationTaskStatus_WhenNoVariationWithoutReason_ReturnsInProgress()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetAdmissionVariationConsultation(false, null);

			project.Details.GetAdmissionVariationConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetAdmissionVariationConsultationTaskStatus_WhenVariationIncluded_ReturnsCompleted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetAdmissionVariationConsultation(true, null);

			project.Details.GetAdmissionVariationConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetAdmissionVariationConsultation_WhenNoVariationAndTierOne_MovesToTierTwo()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					1,
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);


			project.SetAdmissionVariationConsultation(false, "No admission variation required");

			project.Tier.Should().Be(2);
		}
    
    	[Fact]
		public void SetConsultationDuration_ShouldSetDetailsProperties()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation ran for two weeks only");

			project.Details.ConsultationLastedMinimumThreeWeeks.Should().Be(ConsultationDurationAnswer.No);
			project.Details.ConsultationDurationNotMetReason.Should().Be("Consultation ran for two weeks only");
		}

		[Theory]
		[InlineData(ConsultationDurationAnswer.Yes)]
		[InlineData(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut)]
		public void SetConsultationDuration_WhenAnswerIsNotNo_ClearsReason(ConsultationDurationAnswer answer)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(answer, "This should be cleared");

			project.Details.ConsultationLastedMinimumThreeWeeks.Should().Be(answer);
			project.Details.ConsultationDurationNotMetReason.Should().BeNull();
		}

		[Fact]
		public void SetConsultationDuration_WhenNo_AndTierOne_MovesToTierTwo()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					1,
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation was too short");

			project.Tier.Should().Be(2);
		}

		[Theory]
		[InlineData(ConsultationDurationAnswer.Yes)]
		[InlineData(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut)]
		public void SetConsultationDuration_WhenAnswerIsNotNo_DoesNotChangeTier(ConsultationDurationAnswer answer)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					1,
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(answer, null);

			project.Tier.Should().Be(1);
		}

		[Fact]
		public void SetConsultationDuration_WhenTierMovedToTwo_DoesNotRevertToTierOne()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					1,
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation was too short");
			project.SetConsultationDuration(ConsultationDurationAnswer.Yes, null);

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void GetConsultationDurationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetConsultationDurationTaskStatus_WhenNoWithoutReason_ReturnsInProgress()
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(ConsultationDurationAnswer.No, null);

			project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Theory]
		[InlineData(ConsultationDurationAnswer.Yes, null)]
		[InlineData(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut, null)]
		[InlineData(ConsultationDurationAnswer.No, "Consultation ran for two weeks only")]
		public void GetConsultationDurationTaskStatus_WhenAnswered_ReturnsCompleted(
			ConsultationDurationAnswer answer,
			string? reason)
		{
			var project = SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);

			project.SetConsultationDuration(answer, reason);

			project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}
	}
}
