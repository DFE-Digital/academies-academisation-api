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
				SignificantChangeStatus.InProgress,
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
				SignificantChangeStatus.InProgress,
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
		public void SetConsultationDuration_ShouldSetDetailsProperties()
		{
			var project = CreateProject();

			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation ran for two weeks only");

			project.Details.ConsultationLastedMinimumThreeWeeks.Should().Be(ConsultationDurationAnswer.No);
			project.Details.ConsultationDurationNotMetReason.Should().Be("Consultation ran for two weeks only");
		}

		[Theory]
		[InlineData(ConsultationDurationAnswer.Yes)]
		[InlineData(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut)]
		public void SetConsultationDuration_WhenAnswerIsNotNo_ClearsReason(ConsultationDurationAnswer answer)
		{
			var project = CreateProject();

			project.SetConsultationDuration(answer, "This should be cleared");

			project.Details.ConsultationLastedMinimumThreeWeeks.Should().Be(answer);
			project.Details.ConsultationDurationNotMetReason.Should().BeNull();
		}

		[Fact]
		public void SetConsultationDuration_WhenNo_AndTierOne_MovesToTierTwo()
		{
			var project = CreateProject(tier: 1);

			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation was too short");

			project.Tier.Should().Be(2);
		}

		[Theory]
		[InlineData(ConsultationDurationAnswer.Yes)]
		[InlineData(ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut)]
		public void SetConsultationDuration_WhenAnswerIsNotNo_DoesNotChangeTier(ConsultationDurationAnswer answer)
		{
			var project = CreateProject(tier: 1);

			project.SetConsultationDuration(answer, null);

			project.Tier.Should().Be(1);
		}

		[Fact]
		public void SetConsultationDuration_WhenTierMovedToTwo_DoesNotRevertToTierOne()
		{
			var project = CreateProject(tier: 1);

			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation was too short");
			project.SetConsultationDuration(ConsultationDurationAnswer.Yes, null);

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void GetConsultationDurationTaskStatus_WhenNoValues_ReturnsNotStarted()
		{
			var project = CreateProject();

			project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetConsultationDurationTaskStatus_WhenNoWithoutReason_ReturnsInProgress()
		{
			var project = CreateProject();

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
			var project = CreateProject();

			project.SetConsultationDuration(answer, reason);

			project.Details.GetConsultationDurationTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		private SignificantChangeProject CreateProject(byte tier = 1)
		{
			return new SignificantChangeProject(
				SignificantChangeStatus.InProgress,
				_fixture.Create<int>(),
				tier,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());
		}
	}
}
