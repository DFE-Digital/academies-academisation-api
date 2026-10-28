using AutoMapper;
using Dfe.Academies.Academisation.Domain.Core.SignificantChange;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Dfe.Academies.Academisation.Service.Mappers.SignificantChange;
using Dfe.Academies.Academisation.Service.Queries.SignificantChange;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Queries.SignificantChange
{
	public class GetSignificantChangeProjectByIdQueryHandlerTests
	{
		private readonly Mock<ISignificantChangeProjectRepository> _repositoryMock;
		private readonly Mock<ISender> _senderMock;
		private readonly GetSignificantChangeProjectByIdQueryHandler _handler;

		public GetSignificantChangeProjectByIdQueryHandlerTests()
		{
			_repositoryMock = new Mock<ISignificantChangeProjectRepository>();
			var mapperConfiguration = new MapperConfiguration(configuration =>
				configuration.AddProfile<SignificantChangeProjectMappingProfile>());
			mapperConfiguration.AssertConfigurationIsValid();
			var mapper = mapperConfiguration.CreateMapper();
			_senderMock = new Mock<ISender>();

			_handler = new GetSignificantChangeProjectByIdQueryHandler(_repositoryMock.Object, mapper, _senderMock.Object);
		}

		[Fact]
		public async Task Handle_ProjectExists_ReturnsMappedResponse()
		{
			var proposedDecisionDate = DateTime.UtcNow.AddDays(10);
			var proposedChangeDate = DateTime.UtcNow.AddDays(20);

			var query = new GetSignificantChangeProjectByIdQuery(10);
			var cancellationToken = CancellationToken.None;
			var assignedUserId = Guid.NewGuid();

			var project = new SignificantChangeProject(
				SignificantChangeStatus.PreDecision,
				new SignificantChangeProjectOptions(
					urn: 123456,
					tier: 2,
					trustName: "Trust A",
					trustUkprn: "10000001",
					typeOfSignificantChange: "Change of age range",
					schoolName: "School A"
				)
			)
			{
				Id = query.Id
			};

			project.AssignUser(assignedUserId, "assigned.user@test.local", "Assigned User");
			project.SetOtherStakeholderEngagement(
				false,
				"Trust has not consulted stakeholders yet",
				true,
				"Stakeholders objected",
				YesNoNotApplicableAnswer.No,
				"Admissions stakeholders not notified",
				"https://example.com/stakeholder-evidence");
			project.SetConsultationDuration(ConsultationDurationAnswer.No, "Consultation was too short");
			project.SetEqualitiesImpactAssessment(true, null, EqualitiesImpact.SomeImpact, null, "Mitigation", "sharepoint.edu.gov.uk/evidence");
			project.SetReligiousBodyConsultation(
				YesNoNotApplicableAnswer.No,
				"Trust has not consulted religious bodies",
				YesNoNotApplicableAnswer.No,
				"A religious body objected",
				YesNoNotApplicableAnswer.Yes,
				null,
				"https://example.com/religious-evidence");
			project.SetProjectDates(proposedDecisionDate, proposedChangeDate);
			project.SetPlanningPermission(PlanningPermissionAnswer.No, "Awaiting final approval", "Planning permission evidence uploaded");

			_repositoryMock
				.Setup(x => x.GetSignificantChangeProjectById(query.Id, cancellationToken))
				.ReturnsAsync(project);

			var expectedDecisionDate = DateTime.UtcNow.AddDays(-5);
			_senderMock
				.Setup(x => x.Send(It.IsAny<GetSignificantChangeDecisionQuery>(), cancellationToken))
				.ReturnsAsync(new SignificantChangeDecisionServiceModel
				{
					Decision = Decision.Approved,
					DecisionDate = expectedDecisionDate
				});
			var result = await _handler.Handle(query, cancellationToken);

			result.Should().NotBeNull();
			result!.Id.Should().Be(10);
			result.Urn.Should().Be(123456);
			result.SchoolName.Should().Be("School A");
			result.Tier.Should().Be(2);
			result.TrustName.Should().Be("Trust A");
			result.TrustUkprn.Should().Be("10000001");
			result.AssignedUser.Should().BeEquivalentTo(new User(assignedUserId, "Assigned User", "assigned.user@test.local"));
			result.TypeOfSignificantChange.Should().Be("Change of age range");
			result.Status.Should().Be(nameof(SignificantChangeStatus.PreDecision));
			result.Decision.Should().Be(Decision.Approved);
			result.DecisionDate.Should().Be(expectedDecisionDate);

			result.ConsultationDuration.ConsultationLastedMinimumThreeWeeks.Should().Be((int)ConsultationDurationAnswer.No);
			result.ConsultationDuration.ConsultationDurationNotMetReason.Should().Be("Consultation was too short");
			result.ConsultationDuration.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));

			result.EqualitiesImpactAssessment.EqualitiesImpactAssessmentCompleted.Should().BeTrue();
			result.EqualitiesImpactAssessment.EqualitiesImpactAssessmentNoReason.Should().BeNull();
			result.EqualitiesImpactAssessment.EqualitiesImpactIdentified.Should().Be(nameof(EqualitiesImpact.SomeImpact));
			result.EqualitiesImpactAssessment.EqualitiesSomeImpactDetails.Should().Be("Mitigation");
			result.EqualitiesImpactAssessment.EqualitiesImpactSupportingEvidence.Should().Be("sharepoint.edu.gov.uk/evidence");
			result.EqualitiesImpactAssessment.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));
			
			result.ReligiousBodyConsultation.TrustConsultedReligiousBody.Should().Be((int)YesNoNotApplicableAnswer.No);
			result.ReligiousBodyConsultation.TrustConsultedReligiousBodyNotConsultedReason.Should().Be("Trust has not consulted religious bodies");
			result.ReligiousBodyConsultation.ReligiousBodyRaisedObjection.Should().Be((int)YesNoNotApplicableAnswer.No);
			result.ReligiousBodyConsultation.ReligiousBodyRaisedObjectionReason.Should().Be("A religious body objected");
			result.ReligiousBodyConsultation.ReligiousBodyConsentGranted.Should().Be((int)YesNoNotApplicableAnswer.Yes);
			result.ReligiousBodyConsultation.SupportingEvidence.Should().Be("https://example.com/religious-evidence");
			result.ReligiousBodyConsultation.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));

			result.OtherStakeholderEngagement.TrustConsultedStakeholders.Should().BeFalse();
			result.OtherStakeholderEngagement.TrustConsultedStakeholdersNotConsultedReason.Should().Be("Trust has not consulted stakeholders yet");
			result.OtherStakeholderEngagement.WereThereAnyObjectionsRaised.Should().BeTrue();
			result.OtherStakeholderEngagement.StakeholderObjectionsDetails.Should().Be("Stakeholders objected");
			result.OtherStakeholderEngagement.RelevantStakeholdersNotifiedOfAdmissionsChanges.Should().Be((int)YesNoNotApplicableAnswer.No);
			result.OtherStakeholderEngagement.RelevantStakeholdersNotifiedOfAdmissionsChangesReason.Should().Be("Admissions stakeholders not notified");
			result.OtherStakeholderEngagement.SupportingEvidence.Should().Be("https://example.com/stakeholder-evidence");
			result.OtherStakeholderEngagement.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));

			result.ProjectDates.ProposedDecisionDate.Should().Be(proposedDecisionDate);
			result.ProjectDates.ProposedChangeDate.Should().Be(proposedChangeDate);
			result.ProjectDates.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));
			result.PlanningPermission.PlanningPermissionAnswer.Should().Be(PlanningPermissionAnswer.No);
			result.PlanningPermission.AdditionalInformation.Should().Be("Awaiting final approval");
			result.PlanningPermission.SupportingEvidence.Should().Be("Planning permission evidence uploaded");
			result.PlanningPermission.Status.Should().Be(nameof(SignificantChangeTaskStatus.Completed));

		}

		[Fact]
		public async Task Handle_UnansweredTasks_ReturnsNullAnswersAndNotStartedStatuses()
		{
			var query = new GetSignificantChangeProjectByIdQuery(10);
			var project = new SignificantChangeProject(
				SignificantChangeStatus.PreDecision,
				new SignificantChangeProjectOptions(123456, 1, "Trust A", "10000001", "Change of age range", "School A"))
			{
				Id = query.Id
			};

			_repositoryMock.Setup(repository => repository.GetSignificantChangeProjectById(query.Id, CancellationToken.None))
				.ReturnsAsync(project);
			_senderMock.Setup(sender => sender.Send(It.IsAny<GetSignificantChangeDecisionQuery>(), CancellationToken.None))
				.ReturnsAsync(new SignificantChangeDecisionServiceModel());

			var response = await _handler.Handle(query, CancellationToken.None);

			response!.ConsultationDuration.ConsultationLastedMinimumThreeWeeks.Should().BeNull();
			response.ConsultationDuration.Status.Should().Be(nameof(SignificantChangeTaskStatus.NotStarted));
			response.ReligiousBodyConsultation.TrustConsultedReligiousBody.Should().BeNull();
			response.ReligiousBodyConsultation.ReligiousBodyRaisedObjection.Should().BeNull();
			response.ReligiousBodyConsultation.ReligiousBodyConsentGranted.Should().BeNull();
			response.ReligiousBodyConsultation.Status.Should().Be(nameof(SignificantChangeTaskStatus.NotStarted));
			response.OtherStakeholderEngagement.TrustConsultedStakeholders.Should().BeNull();
			response.OtherStakeholderEngagement.WereThereAnyObjectionsRaised.Should().BeNull();
			response.OtherStakeholderEngagement.RelevantStakeholdersNotifiedOfAdmissionsChanges.Should().BeNull();
			response.OtherStakeholderEngagement.Status.Should().Be(nameof(SignificantChangeTaskStatus.NotStarted));
		}

		[Fact]
		public async Task Handle_ProjectDoesNotExist_ReturnsNull()
		{
			var query = new GetSignificantChangeProjectByIdQuery(999);
			using var cts = new CancellationTokenSource();
			var cancellationToken = cts.Token;

			_repositoryMock
				.Setup(x => x.GetSignificantChangeProjectById(query.Id, cancellationToken))
				.ReturnsAsync((SignificantChangeProject?)null);

			var result = await _handler.Handle(query, cancellationToken);

			result.Should().BeNull();
		}
	}
}
