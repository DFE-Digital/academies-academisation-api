using System;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.IService.ServiceModels.SignificantChange;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller;

public static class SignificantChangeDecisionTestData
{
	public static SignificantChangeProjectGetByIdResponse CreateProjectWithCompletedTasks()
	{
		var completed = nameof(SignificantChangeTaskStatus.Completed);
		return new SignificantChangeProjectGetByIdResponse
		{
			SchoolName = "Test School",
			TrustName = "Test Trust",
			TrustUkprn = "12345678",
			TypeOfSignificantChange = "Change of age range",
			ApplicationId = null,
			ApplicationReference = null,
			Status = "PreDecision",
			AssignedUser = new(Guid.NewGuid(), "Test User", "test.user@example.com"),
			ConsultationDuration = new() { Status = completed },
			EqualitiesImpactAssessment = new() { Status = completed },
			ReligiousBodyConsultation = new() { Status = completed },
			ProjectDates = new() { Status = completed },
			LandTransaction = new() { Status = completed },
			OtherStakeholderEngagement = new() { Status = completed },
			Funding = new() { Status = completed },
			PlanningPermission = new() { Status = completed },
			Recommendation = new() { Status = completed },
			HqtfConsideration = new() { Status = completed },
			FhaRisksOrIssuesRaised = new() { Status = completed },
			AdmissionsVariation = new() { Status = completed },
			OfstedInspection = new() { Status = completed },
			LocalAuthorityObjections = new() { Status = completed },
			AdmissionsVariationRecommendation = new() { Status = completed },
			SpecialSchoolSatelliteConsiderations = new () { Status = completed }
		};
	}

	public static TheoryData<Action<SignificantChangeProjectGetByIdResponse, string>, SignificantChangeTaskStatus> IncompleteTasks
	{
		get
		{
			Action<SignificantChangeProjectGetByIdResponse, string>[] setTaskStatuses =
			[
				(project, status) => project.ConsultationDuration.Status = status,
				(project, status) => project.EqualitiesImpactAssessment.Status = status,
				(project, status) => project.ReligiousBodyConsultation.Status = status,
				(project, status) => project.ProjectDates.Status = status,
				(project, status) => project.LandTransaction.Status = status,
				(project, status) => project.OtherStakeholderEngagement.Status = status,
				(project, status) => project.Funding.Status = status,
				(project, status) => project.PlanningPermission.Status = status,
				(project, status) => project.Recommendation.Status = status,
				(project, status) => project.HqtfConsideration.Status = status,
				(project, status) => project.FhaRisksOrIssuesRaised.Status = status,
				(project, status) => project.AdmissionsVariation.Status = status,
				(project, status) => project.OfstedInspection.Status = status,
				(project, status) => project.LocalAuthorityObjections.Status = status,
				(project, status) => project.AdmissionsVariationRecommendation.Status = status
			];
			var data = new TheoryData<Action<SignificantChangeProjectGetByIdResponse, string>, SignificantChangeTaskStatus>();
			foreach (var setTaskStatus in setTaskStatuses)
			{
				data.Add(setTaskStatus, SignificantChangeTaskStatus.NotStarted);
				data.Add(setTaskStatus, SignificantChangeTaskStatus.InProgress);
			}

			return data;
		}
	}
}
