using System;
using AutoFixture;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
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
			var project = CreateProject();

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
			var project = CreateProject();
			var readOnlyDate = DateTime.UtcNow.AddDays(-1);

			project.SetReadOnlyDate(readOnlyDate);

			project.ReadOnlyDate.Should().Be(readOnlyDate);
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

			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No,
				"Trust has not consulted religious bodies",
				YesNoNotApplicableAnswer.Yes,
				null,
				YesNoNotApplicableAnswer.Yes,
				null,
				null);

			project.Details.TrustConsultedReligiousBodyAnswer.Should().Be(YesNoNotApplicableAnswer.No);
			project.Details.TrustConsultedReligiousBodyNotConsultedReason.Should().Be("Trust has not consulted religious bodies");
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var project = CreateProject();
			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenNotConsultedWithoutReason_ReturnsInProgress()
		{
			var project = CreateProject();

			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No, null,
				YesNoNotApplicableAnswer.Yes, null,
				YesNoNotApplicableAnswer.Yes, null, null);

			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenConsulted_ReturnsCompleted()
		{
			var project = CreateProject();

			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.Yes, null,
				YesNoNotApplicableAnswer.No, null,
				YesNoNotApplicableAnswer.Yes, null, null);

			project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetReligiousBodyConsultationTaskStatus_WhenNotConsultedWithReason_ReturnsCompleted()
		{
			var project = CreateProject();

			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No, "Consultation timeline does not allow this yet",
				YesNoNotApplicableAnswer.No, null,
				YesNoNotApplicableAnswer.Yes, null, null);

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

			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No, "No consultation carried out",
				YesNoNotApplicableAnswer.Yes, null,
				YesNoNotApplicableAnswer.Yes, null, null);

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

			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No, "No consultation carried out",
				YesNoNotApplicableAnswer.Yes, null,
				YesNoNotApplicableAnswer.Yes, null, null);
			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.Yes, null,
				YesNoNotApplicableAnswer.Yes, null,
				YesNoNotApplicableAnswer.Yes, null, null);

			project.Tier.Should().Be(2);
		}

		[Theory]
		[InlineData(-1, null)]
		[InlineData(0, null)]
		[InlineData(1, "")]
		[InlineData(2, "  ")]
		public void ReligiousBodyConsultation_StatusRequiresEveryAnswerAndApplicableDetails(int missingDetail, string? text)
		{
			YesNoNotApplicableAnswer?[] answers =
				[null, YesNoNotApplicableAnswer.Yes, YesNoNotApplicableAnswer.No, YesNoNotApplicableAnswer.NotApplicable];

			foreach (var consulted in answers)
			foreach (var objections in answers)
			foreach (var consent in answers)
			{
				var project = CreateProject();
				project.SetReligiousBodyConsultation(
					consulted, missingDetail == 0 ? text : "Consultation details",
					objections, missingDetail == 1 ? text : "Objection details",
					consent, missingDetail == 2 ? text : "Consent details", null);

				var expected = consulted is null && objections is null && consent is null
					? SignificantChangeTaskStatus.NotStarted
					: consulted.HasValue && objections.HasValue && consent.HasValue
					  && (consulted != YesNoNotApplicableAnswer.No || missingDetail != 0)
					  && (objections != YesNoNotApplicableAnswer.Yes || missingDetail != 1)
					  && (consent != YesNoNotApplicableAnswer.No || missingDetail != 2)
						? SignificantChangeTaskStatus.Completed
						: SignificantChangeTaskStatus.InProgress;

				project.Details.GetReligiousBodyConsultationTaskStatus().Should().Be(expected,
					"answers are {0}, {1}, {2}, and missing detail is {3}", consulted, objections, consent, missingDetail);
			}
		}

		[Theory]
		[InlineData(-1, null)]
		[InlineData(0, null)]
		[InlineData(1, "")]
		[InlineData(2, "  ")]
		public void OtherStakeholderEngagement_StatusRequiresEveryAnswerAndApplicableDetails(int missingDetail, string? text)
		{
			bool?[] answers = [null, true, false];
			YesNoNotApplicableAnswer?[] notificationAnswers =
				[null, YesNoNotApplicableAnswer.Yes, YesNoNotApplicableAnswer.No, YesNoNotApplicableAnswer.NotApplicable];

			foreach (var consulted in answers)
			foreach (var objections in answers)
			foreach (var notified in notificationAnswers)
			{
				var project = CreateProject();
				project.SetOtherStakeholderEngagement(
					consulted, missingDetail == 0 ? text : "Consultation details",
					objections, missingDetail == 1 ? text : "Objection details",
					notified, missingDetail == 2 ? text : "Notification details", null);

				var expected = consulted is null && objections is null && notified is null
					? SignificantChangeTaskStatus.NotStarted
					: consulted.HasValue && objections.HasValue && notified.HasValue
					  && (consulted != false || missingDetail != 0)
					  && (objections != true || missingDetail != 1)
					  && (notified != YesNoNotApplicableAnswer.No || missingDetail != 2)
						? SignificantChangeTaskStatus.Completed
						: SignificantChangeTaskStatus.InProgress;

				project.Details.GetOtherStakeholderEngagementTaskStatus().Should().Be(expected,
					"answers are {0}, {1}, {2}, and missing detail is {3}", consulted, objections, notified, missingDetail);
			}
		}

		[Theory]
		[InlineData(null)]
		[InlineData(YesNoNotApplicableAnswer.Yes)]
		[InlineData(YesNoNotApplicableAnswer.No)]
		[InlineData(YesNoNotApplicableAnswer.NotApplicable)]
		public void SetReligiousBodyConsultation_ClearsDetailsThatDoNotApply(YesNoNotApplicableAnswer? answer)
		{
			var project = CreateProject();
			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No, "Old consultation",
				YesNoNotApplicableAnswer.Yes, "Old objection",
				YesNoNotApplicableAnswer.No, "Old consent", null);

			project.SetReligiousBodyConsultation(answer, "Consultation", answer, "Objection", answer, "Consent", "Evidence");

			project.Details.TrustConsultedReligiousBodyNotConsultedReason.Should().Be(answer == YesNoNotApplicableAnswer.No ? "Consultation" : null);
			project.Details.ReligiousBodyRaisedObjectionReason.Should().Be(answer == YesNoNotApplicableAnswer.Yes ? "Objection" : null);
			project.Details.ReligiousBodyConsentNotGrantedReason.Should().Be(answer == YesNoNotApplicableAnswer.No ? "Consent" : null);
			project.Details.ReligiousBodySupportingEvidence.Should().Be("Evidence");
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData(true, YesNoNotApplicableAnswer.Yes)]
		[InlineData(false, YesNoNotApplicableAnswer.No)]
		[InlineData(true, YesNoNotApplicableAnswer.NotApplicable)]
		public void SetOtherStakeholderEngagement_ClearsDetailsThatDoNotApply(bool? answer, YesNoNotApplicableAnswer? notified)
		{
			var project = CreateProject();
			project.SetOtherStakeholderEngagement(false, "Old consultation", true, "Old objection", YesNoNotApplicableAnswer.No, "Old notification", null);

			project.SetOtherStakeholderEngagement(answer, "Consultation", answer, "Objection", notified, "Notification", "Evidence");

			project.Details.TrustConsultedStakeholdersNotConsultedReason.Should().Be(answer == false ? "Consultation" : null);
			project.Details.StakeholderObjectionsDetails.Should().Be(answer == true ? "Objection" : null);
			project.Details.RelevantStakeholdersNotifiedOfAdmissionsChangesReason.Should().Be(notified == YesNoNotApplicableAnswer.No ? "Notification" : null);
			project.Details.OtherStakeholderEngagementSupportingEvidence.Should().Be("Evidence");
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


			project.SetEqualitiesImpactAssessment(equalitiesImpactAssessmentCompleted, null, null, null, null, null);

			project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Theory]
		[InlineData(EqualitiesImpact.Unlikely)]
		[InlineData(EqualitiesImpact.SomeImpact)]
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


			project.SetEqualitiesImpactAssessment(true, null, equalitiesImpact, null, null, null);

			project.Details.GetEqualitiesTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Theory]
		[InlineData(EqualitiesImpact.Likely, "", SignificantChangeTaskStatus.Completed)]
		[InlineData(EqualitiesImpact.Likely, null, SignificantChangeTaskStatus.Completed)]
		[InlineData(EqualitiesImpact.Likely, "Mitigation plan in place",
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


			project.SetEqualitiesImpactAssessment(true, null, impact, mitigation, null, null);

			project.Details.GetEqualitiesTaskStatus().Should().Be(expectedStatus);
		}

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, "some reason")]
		public void SetEqualitiesImpactAssessment_ShouldSetReasonProperty(bool equalitiesImpactAssessmentComplete, string? expectedNoReason)
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


            project.SetEqualitiesImpactAssessment(equalitiesImpactAssessmentComplete, "some reason", null, null, null, null);

            project.Details.EqualitiesImpactAssessmentCompleted.Should().Be(equalitiesImpactAssessmentComplete);
			project.Details.EqualitiesImpactAssessmentNoReason.Should().Be(expectedNoReason);
		}

		[Fact]
		public void SetAdmissionVariationDetails_ShouldSetDetailsProperties()
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

			project.SetAdmissionVariationDetails(
				false,
				"Supporting evidence",
				false,
				false,
				false,
				"Local authority concerns",
				"Place planning concerns",
				"Admissions team concerns");

			project.Details.AdmissionsVariationRequired.Should().BeFalse();
			project.Details.AdmissionsVariationSupportingEvidence.Should().Be("Supporting evidence");
			project.Details.AdmissionsVariationLocalAuthoritySupportive.Should().BeFalse();
			project.Details.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation.Should().Be("Local authority concerns");
			project.Details.AdmissionsVariationPupilPlacePlanningSupportive.Should().BeFalse();
			project.Details.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation.Should().Be("Place planning concerns");
			project.Details.AdmissionsVariationAdmissionsTeamSupportive.Should().BeFalse();
			project.Details.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation.Should().Be("Admissions team concerns");
		}


		[Fact]
		public void GetAdmissionVariationTaskStatus_WhenNoValues_ReturnsNotStarted()
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

			project.Details.GetAdmissionVariationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetAdmissionVariationTaskStatus_WhenNotRequired_ReturnsCompleted()
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

			project.SetAdmissionVariationDetails(false, null, null, null, null, null, null, null);

			project.Details.GetAdmissionVariationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetAdmissionVariationTaskStatus_WhenAllQuestionsAnswered_ReturnsCompleted()
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

			project.SetAdmissionVariationDetails(true, null, true, true, true, null, null, null);

			project.Details.GetAdmissionVariationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
			project.Tier.Should().Be(1);
		}

		[Fact]
		public void SetAdmissionVariationDetails_WhenSupportive_ClearsAdditionalInformation()
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

			project.SetAdmissionVariationDetails(true, null, false, false, false, "LA", "Planning", "Admissions");
			project.SetAdmissionVariationDetails(true, null, true, true, true, "LA", "Planning", "Admissions");

			project.Details.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation.Should().BeNull();
			project.Details.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation.Should().BeNull();
			project.Details.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation.Should().BeNull();
		}

		[Fact]
		public void GetAdmissionVariationTaskStatus_WhenNoAnswerHasNoAdditionalInformation_ReturnsInProgress()
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

			project.SetAdmissionVariationDetails(true, null, false, true, true, null, null, null);

			project.Details.GetAdmissionVariationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Theory]
		[InlineData (false, true, true)]
		[InlineData (true, true, false)]
		[InlineData (true, false, true)]
		public void SetAdmissionVariationDetails_WhenUnsupported_MovesToTierTwo(
			bool admissionsVariationLocalAuthoritySupportive,
			bool admissionsVariationPupilPlacePlanningSupportive,
			bool admissionsVariationAdmissionsTeamSupportive
		)
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


			project.SetAdmissionVariationDetails(
				true,
				"Supporting evidence",
				admissionsVariationLocalAuthoritySupportive,
				admissionsVariationPupilPlacePlanningSupportive,
				admissionsVariationAdmissionsTeamSupportive,
				null,
				null,
				null
			);

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

		[Fact]
		public void SetFunding_WhenAnswerIsNotNo_ClearsAdditionalInformation()
		{
			var project = CreateProject();

			project.SetFunding(FundingAnswer.Yes, "This should be cleared", "Business case");

			project.Details.FundingAnswer.Should().Be(FundingAnswer.Yes);
			project.Details.FundingAdditionalInformation.Should().BeNull();
			project.Details.FundingSupportingEvidence.Should().Be("Business case");
		}

		[Fact]
		public void SetHqtfConsideration_WhenAnswerIsNotYes_ClearsAdditionalInformation()
		{
			var project = CreateProject();

			project.SetHqtfConsideration(HqtfConsiderationAnswer.No, "This should be cleared", "Evidence");

			project.Details.HqtfConsiderationAnswer.Should().Be(HqtfConsiderationAnswer.No);
			project.Details.HqtfConsiderationAdditionalInformation.Should().BeNull();
			project.Details.HqtfConsiderationSupportingEvidence.Should().Be("Evidence");
		}

		[Fact]
		public void SetFhaRisksOrIssuesRaised_WhenAnswerIsNotYes_ClearsAdditionalInformation()
		{
			var project = CreateProject();

			project.SetFhaRisksOrIssuesRaised(false, "This should be cleared", "Evidence");

			project.Details.FhaRisksOrIssuesRaised.Should().BeFalse();
			project.Details.FhaRisksOrIssuesRaisedAdditionalInformation.Should().BeNull();
			project.Details.FhaRisksOrIssuesRaisedSupportingEvidence.Should().Be("Evidence");
		}

		[Theory]
		[InlineData(null, null, null, SignificantChangeTaskStatus.NotStarted)]
		[InlineData(true, "Risks need consideration", "Evidence", SignificantChangeTaskStatus.Completed)]
		[InlineData(false, null, null, SignificantChangeTaskStatus.Completed)]
		public void GetFhaRisksOrIssuesRaisedTaskStatus_ReturnsExpectedStatus(
			bool? answer,
			string? additionalInformation,
			string? supportingEvidence,
			SignificantChangeTaskStatus expected)
		{
			var project = CreateProject();

			project.SetFhaRisksOrIssuesRaised(answer, additionalInformation, supportingEvidence);

			project.Details.GetFhaRisksOrIssuesRaisedTaskStatus().Should().Be(expected);
		}

		[Theory]
		[InlineData(null, null, null, SignificantChangeTaskStatus.NotStarted)]
		[InlineData(HqtfConsiderationAnswer.Yes, "Not considered", "Evidence", SignificantChangeTaskStatus.Completed)]
		[InlineData(HqtfConsiderationAnswer.No, null, null, SignificantChangeTaskStatus.Completed)]
		[InlineData(HqtfConsiderationAnswer.No, "Not considered", null, SignificantChangeTaskStatus.Completed)]
		[InlineData(HqtfConsiderationAnswer.NotApplicable, null, "Not applicable evidence", SignificantChangeTaskStatus.Completed)]
		public void GetHqtfConsiderationTaskStatus_ReturnsExpectedStatus(
			HqtfConsiderationAnswer? answer,
			string? additionalInformation,
			string? supportingEvidence,
			SignificantChangeTaskStatus expected)
		{
			var project = CreateProject();

			project.SetHqtfConsideration(answer, additionalInformation, supportingEvidence);

			project.Details.GetHqtfConsiderationTaskStatus().Should().Be(expected);
		}

		[Theory]
		[InlineData(null, null, null, SignificantChangeTaskStatus.NotStarted)]
		[InlineData(FundingAnswer.No, null, "Business case", SignificantChangeTaskStatus.InProgress)]
		[InlineData(FundingAnswer.No, "Funding is unavailable", "Business case", SignificantChangeTaskStatus.Completed)]
		[InlineData(FundingAnswer.Yes, null, null, SignificantChangeTaskStatus.Completed)]
		[InlineData(FundingAnswer.No, "Funding is unavailable", null, SignificantChangeTaskStatus.Completed)]
		[InlineData(FundingAnswer.NotApplicable, null, "Not applicable evidence", SignificantChangeTaskStatus.Completed)]
		public void GetFundingTaskStatus_ReturnsExpectedStatus(
			FundingAnswer? answer,
			string? additionalInformation,
			string? supportingEvidence,
			SignificantChangeTaskStatus expected)
		{
			var project = CreateProject();

			project.SetFunding(answer, additionalInformation, supportingEvidence);

			project.Details.GetFundingTaskStatus().Should().Be(expected);
		}

		[Fact]
		public void SetOfstedInspection_ShouldSetDetailsAndClearAdditionalInformationWhenStandardsAreMet()
		{
			var project = CreateProject();

			var inspectionDate = new DateTime(2026, 10, 20);

			project.SetOfstedInspection(inspectionDate, true, "Additional information", "Supporting evidence");

			project.Details.OfstedInspectionDate.Should().Be(inspectionDate);
			project.Details.MeetsOfstedStandards.Should().BeTrue();
			project.Details.OfstedAdditionalInformation.Should().BeNull();
			project.Details.OfstedSupportingEvidence.Should().Be("Supporting evidence");
			project.Details.GetOfstedTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}
		
		[Fact]
		public void SetRecommendation_WhenAnswersProvided_SetsAnswers()
		{
			var project = CreateProject();

			project.SetRecommendation(Recommendation.Approve, "This is the recommendation comment");

			project.Details.Recommendation.Should().Be(Recommendation.Approve);
			project.Details.RecommendationMoreInformation.Should().Be("This is the recommendation comment");
		}

		[Theory]
		[InlineData(null, null, SignificantChangeTaskStatus.NotStarted)]
		[InlineData(Recommendation.Decline, null, SignificantChangeTaskStatus.Completed)]
		[InlineData(null, "Blah blah 6 7", SignificantChangeTaskStatus.InProgress)]
		[InlineData(Recommendation.Approve, "Yeah, I highly reommend approving because it's a really good idea", SignificantChangeTaskStatus.Completed)]
		public void GetRecommendationTaskStatus_ReturnsExpectedStatus(
			Recommendation? answer,
			string? moreInformation,
			SignificantChangeTaskStatus expected)
		{
			var project = CreateProject();

			project.SetRecommendation(answer, moreInformation);

			project.Details.GetRecommendationTaskStatus().Should().Be(expected);
		}

		private SignificantChangeProject CreateProject()
		{
			return SignificantChangeProject.Create(
				new SignificantChangeProjectOptions(
					_fixture.Create<int>(),
					_fixture.Create<byte>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>(),
					_fixture.Create<string>()),
				DateTime.UtcNow);
		}
	}
}
