﻿using System.Globalization;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;
using Dfe.Academies.Academisation.IService.ServiceModels.TransferProject;

namespace Dfe.Academies.Academisation.Service.Mappers.TransferProject
{
	public static class AcademyTransferProjectResponseFactory
	{
		public static AcademyTransferProjectResponse Create(ITransferProject model)
		{
			if (model == null)
			{
				return null!;
			}
			
			var transferringAcademies = model.TransferringAcademies
				.Select(a => new TransferringAcademyDto
				{
					IncomingTrustUkprn = a.IncomingTrustUkprn,
					IncomingTrustName = a.IncomingTrustName,
					OutgoingAcademyUkprn = a.OutgoingAcademyUkprn,
					PupilNumbersAdditionalInformation = a.PupilNumbersAdditionalInformation,
					LatestOfstedReportAdditionalInformation = a.LatestOfstedReportAdditionalInformation,
					KeyStage2PerformanceAdditionalInformation = a.KeyStage2PerformanceAdditionalInformation,
					KeyStage4PerformanceAdditionalInformation = a.KeyStage4PerformanceAdditionalInformation,
					KeyStage5PerformanceAdditionalInformation = a.KeyStage5PerformanceAdditionalInformation,
					PFIScheme = a.PFIScheme,
					PFISchemeDetails = a.PFISchemeDetails,
					DistanceFromAcademyToTrustHq = a.DistanceFromAcademyToTrustHq,
					DistanceFromAcademyToTrustHqDetails = a.DistanceFromAcademyToTrustHqDetails,
					ViabilityIssues = a.ViabilityIssues,
					FinancialDeficit = a.FinancialDeficit,
					MPNameAndParty = a.MPNameAndParty,
					PublishedAdmissionNumber = a.PublishedAdmissionNumber,
					LocalAuthority = a.LocalAuthority,
					Region = a.Region
				})
				.ToList();

			var features = new AcademyTransferProjectFeaturesResponse
			{
				WhoInitiatedTheTransfer = model.WhoInitiatedTheTransfer!,
				SpecificReasonsForTransfer = model.SpecificReasonsForTransfer.ToList(),
				RddOrEsfaIntervention = model.RddOrEsfaIntervention,
				RddOrEsfaInterventionDetail = model.RddOrEsfaInterventionDetail!,
				TypeOfTransfer = model.TypeOfTransfer!,
				OtherTransferTypeDescription = model.OtherTransferTypeDescription!,
				IsCompleted = model.FeatureSectionIsCompleted
			};

			var dates = new AcademyTransferProjectDatesResponse
			{
				TransferFirstDiscussed =
					model.TransferFirstDiscussed?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)!,
				TargetDateForTransfer =
					model.TargetDateForTransfer?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)!,
				HtbDate = model.HtbDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)!,
				PreviousAdvisoryBoardDate = model.PreviousAdvisoryBoardDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)!,
				HasHtbDate = model.HasHtbDate,
				IsCompleted = model.TransferDatesSectionIsCompleted,
				HasTargetDateForTransfer = model.HasTargetDateForTransfer,
				HasTransferFirstDiscussedDate = model.HasTransferFirstDiscussedDate
			};

			var intendedTransferBenefits = new IntendedTransferBenefitResponse
			{
				OtherBenefitValue = model.OtherBenefitValue!,
				SelectedBenefits = model.IntendedTransferBenefits
					.Select(b => b.SelectedBenefit).ToList()
			};

			var otherFactorsToConsider = new OtherFactorsToConsiderResponse
			{
				HighProfile = new BenefitConsideredFactorResponse
				{
					ShouldBeConsidered = model.HighProfileShouldBeConsidered,
					FurtherSpecification = model.HighProfileFurtherSpecification!
				},
				ComplexLandAndBuilding = new BenefitConsideredFactorResponse
				{
					ShouldBeConsidered = model.ComplexLandAndBuildingShouldBeConsidered,
					FurtherSpecification = model.ComplexLandAndBuildingFurtherSpecification!
				},
				FinanceAndDebt = new BenefitConsideredFactorResponse
				{
					ShouldBeConsidered = model.FinanceAndDebtShouldBeConsidered,
					FurtherSpecification = model.FinanceAndDebtFurtherSpecification!
				},
				OtherRisks = new BenefitConsideredFactorResponse
				{
					ShouldBeConsidered = model.OtherRisksShouldBeConsidered,
					FurtherSpecification = model.OtherRisksFurtherSpecification!
				}
			};

			return new AcademyTransferProjectResponse
			{
				Id = model.Id,
				ProjectUrn = model.Urn.ToString(),
				ProjectReference = model.ProjectReference!,
				OutgoingTrustUkprn = model.OutgoingTrustUkprn,
				OutgoingTrustName = model.OutgoingTrustName!,
				TransferringAcademies = transferringAcademies,
				Features = features,
				Dates = dates,
				Benefits = new AcademyTransferProjectBenefitsResponse
				{
					IntendedTransferBenefits = intendedTransferBenefits,
					OtherFactorsToConsider = otherFactorsToConsider,
					EqualitiesImpactAssessmentConsidered = model.EqualitiesImpactAssessmentConsidered,
					IsCompleted = model.BenefitsSectionIsCompleted,
					AnyRisks = model.AnyRisks
				},
				LegalRequirements = new AcademyTransferProjectLegalRequirementsResponse
				{
					IncomingTrustAgreement = model.IncomingTrustAgreement!,
					DiocesanConsent = model.DiocesanConsent!,
					OutgoingTrustConsent = model.OutgoingTrustConsent!,
					IsCompleted = model.LegalRequirementsSectionIsCompleted,
				},
				Rationale = new AcademyTransferProjectRationaleResponse
				{
					ProjectRationale = model.ProjectRationale!,
					TrustSponsorRationale = model.TrustSponsorRationale!,
					IsCompleted = model.RationaleSectionIsCompleted
				},
				GeneralInformation = new AcademyTransferProjectGeneralInformationResponse
				{
					Author = model.Author!,
					Recommendation = model.Recommendation!
				},
				AssignedUser = string.IsNullOrWhiteSpace(model.AssignedUserEmailAddress)
				? null
				: new AssignedUserResponse
				{
					FullName = model.AssignedUserFullName,
					EmailAddress = model.AssignedUserEmailAddress,
					Id = model.AssignedUserId
				},
				State = model.State!,
				Status = model.Status!,
				IsFormAMat = model.IsFormAMat,
				IsReadOnly = model.ReadOnlyDate.HasValue,
				ProjectSentToCompleteDate = model.ReadOnlyDate,
				IncomingTrustReferenceNumber = model.IncomingTrustReferenceNumber
				
			};
		}
	}
}
