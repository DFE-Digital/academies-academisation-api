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
		public void SetConsultStakeholders_ShouldSetDetailsProperties()
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

			project.SetConsultStakeholders(false, "Trust has not consulted stakeholders yet");

			project.Details.TrustConsultedStakeholders.Should().BeFalse();
			project.Details.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Trust has not consulted stakeholders yet");
		}

		[Fact]
		public void GetConsultStakeholdersTaskStatus_WhenNoValues_ReturnsNotStarted()
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

			project.Details.GetConsultStakeholdersTaskStatus().Should().Be(SignificantChangeTaskStatus.NotStarted);
		}

		[Fact]
		public void GetConsultStakeholdersTaskStatus_WhenNotConsultedWithoutReason_ReturnsInProgress()
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

			project.SetConsultStakeholders(false, null);

			project.Details.GetConsultStakeholdersTaskStatus().Should().Be(SignificantChangeTaskStatus.InProgress);
		}

		[Fact]
		public void GetConsultStakeholdersTaskStatus_WhenConsulted_ReturnsCompleted()
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

			project.SetConsultStakeholders(true, null);

			project.Details.GetConsultStakeholdersTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void GetConsultStakeholdersTaskStatus_WhenNotConsultedWithReason_ReturnsCompleted()
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

			project.SetConsultStakeholders(false, "Consultation timeline does not allow this yet");

			project.Details.GetConsultStakeholdersTaskStatus().Should().Be(SignificantChangeTaskStatus.Completed);
		}

		[Fact]
		public void SetConsultStakeholders_WhenNotConsulted_AndTierOne_MovesToTierTwo()
		{
			var project = new SignificantChangeProject(
				SignificantChangeStatus.InProgress,
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());

			project.SetConsultStakeholders(false, "No consultation carried out");

			project.Tier.Should().Be(2);
		}

		[Fact]
		public void SetConsultStakeholders_WhenTierMovedToTwo_DoesNotRevertToTierOne()
		{
			var project = new SignificantChangeProject(
				SignificantChangeStatus.InProgress,
				_fixture.Create<int>(),
				(byte)1,
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>(),
				_fixture.Create<string>());

			project.SetConsultStakeholders(false, "No consultation carried out");
			project.SetConsultStakeholders(true, null);

			project.Tier.Should().Be(2);
		}
	}
}
