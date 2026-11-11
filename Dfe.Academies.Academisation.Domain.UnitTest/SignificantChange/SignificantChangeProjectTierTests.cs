using System;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using FluentAssertions;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.SignificantChange
{
	public class SignificantChangeProjectTierTests
	{
		public enum TierTwoTrigger
		{
			AdmissionsVariationLocalAuthorityNotSupportive,
			AdmissionsVariationPupilPlacePlanningNotSupportive,
			AdmissionsVariationAdmissionsTeamNotSupportive,
			ConsultationDurationExplicitNo,
			TrustNotConsultedStakeholders,
			StakeholderObjectionsRaised,
			RelevantStakeholdersNotNotifiedOfAdmissionsChanges,
			LocalAuthorityRaisedObjections,
			ReligiousBodyNotConsulted,
			ReligiousBodyRaisedObjection,
			ReligiousBodyConsentNotGranted,
			OfstedStandardsNotMet,
			EqualitiesImpactAssessmentNotCompleted,
			EqualitiesImpactLikely,
			EqualitiesImpactSomeImpact,
			LandTransactionApplicationNotSubmitted,
			LandTransactionConsentNotGranted,
			PlanningPermissionNotSecured,
			FundingNotSecured,
			FhaRisksOrIssuesRaised,
			HqtfIssuesOrRisksIdentified
		}

		[Fact]
		public void SetProjectTier_WhenAllAnswersAreTierOne_SetsTierOne()
		{
			var project = CreateTierOneProject(tier: 2);

			project.SetProjectTier();

			project.Tier.Should().Be(1);
		}

		[Fact]
		public void SetProjectTier_WhenConsultationIsNoSatisfactoryConsultationCarriedOut_SetsTierOne()
		{
			var project = CreateTierOneProject();
			project.Details.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut;

			project.SetProjectTier();

			project.Tier.Should().Be(1);
		}

		[Theory]
		[MemberData(nameof(TierTwoTriggerScenarios))]
		public void SetProjectTier_WhenSingleTierTwoTriggerIsMet_SetsTierTwo(
			string scenario,
			TierTwoTrigger trigger)
		{
			var project = CreateTierOneProject();
			ApplyTierTwoTrigger(project, trigger);

			project.SetProjectTier();

			project.Tier.Should().Be(2, scenario);
		}

		public static TheoryData<string, TierTwoTrigger> TierTwoTriggerScenarios =>
			new()
			{
				{
					"admissions variation local authority is not supportive",
					TierTwoTrigger.AdmissionsVariationLocalAuthorityNotSupportive
				},
				{
					"admissions variation pupil place planning team is not supportive",
					TierTwoTrigger.AdmissionsVariationPupilPlacePlanningNotSupportive
				},
				{
					"admissions variation admissions team is not supportive",
					TierTwoTrigger.AdmissionsVariationAdmissionsTeamNotSupportive
				},
				{
					"consultation duration explicit no",
					TierTwoTrigger.ConsultationDurationExplicitNo
				},
				{
					"trust has not consulted stakeholders",
					TierTwoTrigger.TrustNotConsultedStakeholders
				},
				{
					"stakeholder objections were raised",
					TierTwoTrigger.StakeholderObjectionsRaised
				},
				{
					"relevant stakeholders were not notified of admissions changes",
					TierTwoTrigger.RelevantStakeholdersNotNotifiedOfAdmissionsChanges
				},
				{
					"local authority has raised objections",
					TierTwoTrigger.LocalAuthorityRaisedObjections
				},
				{
					"religious body was not consulted",
					TierTwoTrigger.ReligiousBodyNotConsulted
				},
				{
					"religious body has raised an objection",
					TierTwoTrigger.ReligiousBodyRaisedObjection
				},
				{
					"religious body consent was not granted",
					TierTwoTrigger.ReligiousBodyConsentNotGranted
				},
				{
					"ofsted standards are not met",
					TierTwoTrigger.OfstedStandardsNotMet
				},
				{
					"equalities impact assessment has not been completed",
					TierTwoTrigger.EqualitiesImpactAssessmentNotCompleted
				},
				{
					"equalities impact is likely",
					TierTwoTrigger.EqualitiesImpactLikely
				},
				{
					"equalities impact has some impact",
					TierTwoTrigger.EqualitiesImpactSomeImpact
				},
				{
					"land transaction application has not been submitted",
					TierTwoTrigger.LandTransactionApplicationNotSubmitted
				},
				{
					"land transaction consent has not been granted",
					TierTwoTrigger.LandTransactionConsentNotGranted
				},
				{
					"planning permission has not been secured",
					TierTwoTrigger.PlanningPermissionNotSecured
				},
				{
					"funding has not been secured",
					TierTwoTrigger.FundingNotSecured
				},
				{
					"fha risks or issues have been raised",
					TierTwoTrigger.FhaRisksOrIssuesRaised
				},
				{
					"hqtf issues or risks have been identified",
					TierTwoTrigger.HqtfIssuesOrRisksIdentified
				}
			};

		private static void ApplyTierTwoTrigger(SignificantChangeProject project, TierTwoTrigger trigger)
		{
			switch (trigger)
			{
				case TierTwoTrigger.AdmissionsVariationLocalAuthorityNotSupportive:
					project.Details.AdmissionsVariationLocalAuthoritySupportive = false;
					break;
				case TierTwoTrigger.AdmissionsVariationPupilPlacePlanningNotSupportive:
					project.Details.AdmissionsVariationPupilPlacePlanningSupportive = false;
					break;
				case TierTwoTrigger.AdmissionsVariationAdmissionsTeamNotSupportive:
					project.Details.AdmissionsVariationAdmissionsTeamSupportive = false;
					break;
				case TierTwoTrigger.ConsultationDurationExplicitNo:
					project.Details.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.No;
					break;
				case TierTwoTrigger.TrustNotConsultedStakeholders:
					project.Details.TrustConsultedStakeholders = false;
					break;
				case TierTwoTrigger.StakeholderObjectionsRaised:
					project.Details.WereThereAnyObjectionsRaised = true;
					break;
				case TierTwoTrigger.RelevantStakeholdersNotNotifiedOfAdmissionsChanges:
					project.Details.RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.No;
					break;
				case TierTwoTrigger.LocalAuthorityRaisedObjections:
					project.Details.LocalAuthorityRaisedObjections = true;
					break;
				case TierTwoTrigger.ReligiousBodyNotConsulted:
					project.Details.TrustConsultedReligiousBodyAnswer = YesNoNotApplicableAnswer.No;
					break;
				case TierTwoTrigger.ReligiousBodyRaisedObjection:
					project.Details.ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.Yes;
					break;
				case TierTwoTrigger.ReligiousBodyConsentNotGranted:
					project.Details.ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.No;
					break;
				case TierTwoTrigger.OfstedStandardsNotMet:
					project.Details.MeetsOfstedStandards = false;
					break;
				case TierTwoTrigger.EqualitiesImpactAssessmentNotCompleted:
					project.Details.EqualitiesImpactAssessmentCompleted = false;
					break;
				case TierTwoTrigger.EqualitiesImpactLikely:
					project.Details.EqualitiesImpactIdentified = EqualitiesImpact.Likely;
					break;
				case TierTwoTrigger.EqualitiesImpactSomeImpact:
					project.Details.EqualitiesImpactIdentified = EqualitiesImpact.SomeImpact;
					break;
				case TierTwoTrigger.LandTransactionApplicationNotSubmitted:
					project.Details.LandTransactionApplication = YesNoNotApplicableAnswer.No;
					break;
				case TierTwoTrigger.LandTransactionConsentNotGranted:
					project.Details.LandTransactionConsent = YesNoNotApplicableAnswer.No;
					break;
				case TierTwoTrigger.PlanningPermissionNotSecured:
					project.Details.PlanningPermission = PlanningPermissionAnswer.No;
					break;
				case TierTwoTrigger.FundingNotSecured:
					project.Details.FundingAnswer = FundingAnswer.No;
					break;
				case TierTwoTrigger.FhaRisksOrIssuesRaised:
					project.Details.FhaRisksOrIssuesRaised = true;
					break;
				case TierTwoTrigger.HqtfIssuesOrRisksIdentified:
					project.Details.HqtfConsiderationAnswer = HqtfConsiderationAnswer.Yes;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(trigger), trigger, "Unknown tier-two trigger");
			}
		}

		private static SignificantChangeProject CreateTierOneProject(byte tier = 1)
		{
			var project = CreateProject(tier);

			project.Details.AdmissionsVariationRequired = true;
			project.Details.AdmissionsVariationLocalAuthoritySupportive = true;
			project.Details.AdmissionsVariationPupilPlacePlanningSupportive = true;
			project.Details.AdmissionsVariationAdmissionsTeamSupportive = true;
			project.Details.ConsultationLastedMinimumThreeWeeks = ConsultationDurationAnswer.Yes;
			project.Details.TrustConsultedStakeholders = true;
			project.Details.WereThereAnyObjectionsRaised = false;
			project.Details.RelevantStakeholdersNotifiedOfAdmissionsChanges = YesNoNotApplicableAnswer.Yes;
			project.Details.LocalAuthorityRaisedObjections = false;
			project.Details.TrustConsultedReligiousBodyAnswer = YesNoNotApplicableAnswer.Yes;
			project.Details.ReligiousBodyRaisedObjection = YesNoNotApplicableAnswer.No;
			project.Details.ReligiousBodyConsentGranted = YesNoNotApplicableAnswer.Yes;
			project.Details.MeetsOfstedStandards = true;
			project.Details.EqualitiesImpactAssessmentCompleted = true;
			project.Details.EqualitiesImpactIdentified = EqualitiesImpact.Unlikely;
			project.Details.LandTransactionApplication = YesNoNotApplicableAnswer.Yes;
			project.Details.LandTransactionConsent = YesNoNotApplicableAnswer.Yes;
			project.Details.PlanningPermission = PlanningPermissionAnswer.Yes;
			project.Details.FundingAnswer = FundingAnswer.Yes;
			project.Details.FhaRisksOrIssuesRaised = false;
			project.Details.HqtfConsiderationAnswer = HqtfConsiderationAnswer.No;

			return project;
		}

		private static SignificantChangeProject CreateProject(byte tier = 1)
		{
			return new SignificantChangeProject(
				SignificantChangeStatus.PreDecision,
				new SignificantChangeProjectOptions(
					urn: 123456,
					tier: tier,
					trustName: "Example trust",
					trustUkprn: "10012345",
					typeOfSignificantChange: "example",
					schoolName: "Example school"));
		}
	}
}