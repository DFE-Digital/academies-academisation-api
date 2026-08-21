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
			var schoolName= _fixture.Create<string>();

			var project = new SignificantChangeProject(status, urn, tier, trustName, trustUkprn, typeOfSignificantChange, schoolName);

			project.Status.Should().Be(status);
			project.Urn.Should().Be(urn);
			project.Tier.Should().Be(tier);
			project.TrustName.Should().Be(trustName);
			project.TrustUkprn.Should().Be(trustUkprn);
			project.TypeOfSignificantChange.Should().Be(typeOfSignificantChange);
			project.AssignedUserId.Should().BeNull();
			project.AssignedUserFullName.Should().BeNull();
			project.AssignedUserEmailAddress.Should().BeNull();
		}

		[Fact]
		public void AssignUser_ShouldSetUserProperties()
		{
			var project = new SignificantChangeProject(
				_fixture.Create<SignificantChangeStatus>(),
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			var userId = _fixture.Create<Guid>();
			var userEmail = _fixture.Create<string>();
			var userFullName = _fixture.Create<string>();

			project.AssignUser(userId, userEmail, userFullName);

			project.AssignedUserId.Should().Be(userId);
			project.AssignedUserEmailAddress.Should().Be(userEmail);
			project.AssignedUserFullName.Should().Be(userFullName);
		}
    
		[Fact]
		public void SetReadOnlyDate_ShouldSetReadOnlyDate()
		{
			var project = SignificantChangeProject.Create(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				DateTime.UtcNow);

			var readOnlyDate = DateTime.UtcNow.AddDays(-1);

			project.SetReadOnlyDate(readOnlyDate);

			project.ReadOnlyDate.Should().Be(readOnlyDate);
		}

		[Fact]
		public void SetStakeholderConsultation_ShouldSetDetailsProperties()
		{
			var project = new SignificantChangeProject(
				_fixture.Create<SignificantChangeStatus>(),
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			project.SetStakeholderConsultation(false, "Trust has not consulted stakeholders yet");

			project.Details.TrustConsultedStakeholders.Should().BeFalse();
			project.Details.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Trust has not consulted stakeholders yet");
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var project = new SignificantChangeProject(
				_fixture.Create<SignificantChangeStatus>(),
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenNotConsultedWithoutReason_ReturnsInProgress()
		{
			var project = new SignificantChangeProject(
				_fixture.Create<SignificantChangeStatus>(),
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			project.SetStakeholderConsultation(false, null);

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenConsulted_ReturnsCompleted()
		{
			var project = new SignificantChangeProject(
				_fixture.Create<SignificantChangeStatus>(),
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			project.SetStakeholderConsultation(true, null);

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetStakeholderConsultationTaskStatus_WhenNotConsultedWithReason_ReturnsCompleted()
		{
			var project = new SignificantChangeProject(
				_fixture.Create<SignificantChangeStatus>(),
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>()
			);

			project.SetStakeholderConsultation(false, "Consultation timeline does not allow this yet");

			project.Details.GetStakeholderConsultationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetStakeholderConsultation_WhenNotConsulted_AndTierOne_MovesToTierTwo()
		{
			var project = new SignificantChangeProject(
				SignificantChangeStatus.PreDecision,
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());

			project.SetStakeholderConsultation(false, "No consultation carried out");

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetStakeholderConsultation_WhenTierMovedToTwo_DoesNotRevertToTierOne()
		{
			var project = new SignificantChangeProject(
				SignificantChangeStatus.PreDecision,
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());

			project.SetStakeholderConsultation(false, "No consultation carried out");
			project.SetStakeholderConsultation(true, null);

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetProjectDates_ShouldSetDates()
		{
			var project = SignificantChangeProject.Create(
				_fixture.Create<int>(),
				_fixture.Create<byte>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
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
        public void GetProjectDates_ShouldHaveCorrectStatus(string? proposedDecisionDateString, string? proposedChangeDateString, SignificantChangeTaskStatus expectedTaskStatus)
        {
            var project = SignificantChangeProject.Create(
            _fixture.Create<int>(),
            _fixture.Create<byte>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            DateTime.UtcNow);

            DateTime? proposedDecisionDate = string.IsNullOrEmpty(proposedDecisionDateString) ? null : DateTime.Parse(proposedDecisionDateString);
            DateTime? proposedChangeDate = string.IsNullOrEmpty(proposedChangeDateString) ? null : DateTime.Parse(proposedChangeDateString);

            project.Details.ProposedDecisionDate = proposedDecisionDate;
            project.Details.ProposedChangeDate = proposedChangeDate;

            project.Details.GetConfirmProjectDatesTaskStatus().Should().Be(expectedTaskStatus);
        }
	}
}
