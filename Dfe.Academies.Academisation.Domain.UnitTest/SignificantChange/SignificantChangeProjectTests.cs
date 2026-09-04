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

			var project = new SignificantChangeProject(status,significantChangeProjectOptions);

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
			project.Details.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Trust has not consulted stakeholders yet");
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

			var project = new SignificantChangeProject(SignificantChangeStatus.PreDecision, significantChangeProjectOptions);

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
	}
}
