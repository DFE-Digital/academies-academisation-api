
using System.ComponentModel.DataAnnotations;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferProject : IAggregateRoot
	{
		private TransferProject()
		{
			_intendedTransferBenefits =
				new List<IntendedTransferBenefit>();
			_transferringAcademies = new List<TransferringAcademy>();
		}

		public int Id { get; private set; }
		public int Urn { get; private set; }

		public string ProjectReference { get; private set; }
		public string OutgoingTrustUkprn { get; private set; }
		public string WhoInitiatedTheTransfer { get; private set; }
		public bool? RddOrEsfaIntervention { get; private set; }
		public string RddOrEsfaInterventionDetail { get; private set; }
		public string TypeOfTransfer { get; private set; }
		public string OtherTransferTypeDescription { get; private set; }
		public DateTime? TransferFirstDiscussed { get; private set; }
		public DateTime? TargetDateForTransfer { get; private set; }
		public DateTime? HtbDate { get; private set; }
		public bool? HasTransferFirstDiscussedDate { get; private set; }
		public bool? HasTargetDateForTransfer { get; private set; }
		public bool? HasHtbDate { get; private set; }
		public string ProjectRationale { get; private set; }
		public string TrustSponsorRationale { get; private set; }
		public string State { get; private set; }
		public string Status { get; private set; }

		public bool? AnyRisks { get; private set; }
		public bool? HighProfileShouldBeConsidered { get; private set; }
		public string HighProfileFurtherSpecification { get; private set; }
		public bool? ComplexLandAndBuildingShouldBeConsidered { get; private set; }
		public string ComplexLandAndBuildingFurtherSpecification { get; private set; }
		public bool? FinanceAndDebtShouldBeConsidered { get; private set; }
		public string FinanceAndDebtFurtherSpecification { get; private set; }

		public bool? OtherRisksShouldBeConsidered { get; private set; }

		public bool? EqualitiesImpactAssessmentConsidered { get; private set; }

		[MaxLength(20000)]
		public string OtherRisksFurtherSpecification { get; private set; }
		public string OtherBenefitValue { get; private set; }
		public string Author { get; private set; }
		public string Recommendation { get; private set; }
		public string IncomingTrustAgreement { get; private set; }
		public string DiocesanConsent { get; private set; }
		public string OutgoingTrustConsent { get; private set; }
		public bool? LegalRequirementsSectionIsCompleted { get; private set; }
		public bool? FeatureSectionIsCompleted { get; private set; }
		public bool? BenefitsSectionIsCompleted { get; private set; }
		public bool? RationaleSectionIsCompleted { get; private set; }
		public string AssignedUserFullName { get; private set; }
		public string AssignedUserEmailAddress { get; private set; }
		public Guid? AssignedUserId { get; private set; }

		private readonly List<IntendedTransferBenefit> _intendedTransferBenefits;
		public IReadOnlyCollection<IntendedTransferBenefit> IntendedTransferBenefits => _intendedTransferBenefits;

		private readonly List<TransferringAcademy> _transferringAcademies;
		public IReadOnlyCollection<TransferringAcademy> TransferringAcademies => _transferringAcademies;

		public DateTime? CreatedOn { get; private set; }

		public static TransferProject Create(AcademyTransferProjectRequest request)
		{
			var transferFirstDiscussed = ParseDate(request?.Dates?.TransferFirstDiscussed);
			var targetDateForTransfer = ParseDate(request?.Dates?.TargetDateForTransfer);
			var htbDate = ParseDate(request?.Dates?.HtbDate);

			return new TransferProject
			{
				ProjectReference = request?.ProjectReference,
				OutgoingTrustUkprn = request.OutgoingTrustUkprn,
				WhoInitiatedTheTransfer = request.Features?.WhoInitiatedTheTransfer,
				RddOrEsfaIntervention = request.Features?.RddOrEsfaIntervention,
				RddOrEsfaInterventionDetail = request.Features?.RddOrEsfaInterventionDetail,
				TypeOfTransfer = request.Features?.TypeOfTransfer,
				OtherTransferTypeDescription = request.Features?.OtherTransferTypeDescription,
				TransferFirstDiscussed = transferFirstDiscussed,
				TargetDateForTransfer = targetDateForTransfer,
				HtbDate = htbDate,
				ProjectRationale = request.Rationale?.ProjectRationale,
				TrustSponsorRationale = request.Rationale?.TrustSponsorRationale,
				State = request.State,
				Status = request.Status,
				Author = request.GeneralInformation?.Author,
				Recommendation = request.GeneralInformation?.Recommendation,
				AnyRisks = request.Benefits?.AnyRisks,
				HighProfileShouldBeConsidered = request.Benefits?.OtherFactorsToConsider?.HighProfile?.ShouldBeConsidered,
				HighProfileFurtherSpecification = request.Benefits?.OtherFactorsToConsider?.HighProfile?.FurtherSpecification,
				ComplexLandAndBuildingShouldBeConsidered = request.Benefits?.OtherFactorsToConsider?.ComplexLandAndBuilding?.ShouldBeConsidered,
				ComplexLandAndBuildingFurtherSpecification = request.Benefits?.OtherFactorsToConsider?.ComplexLandAndBuilding?.FurtherSpecification,
				FinanceAndDebtShouldBeConsidered = request.Benefits?.OtherFactorsToConsider?.FinanceAndDebt?.ShouldBeConsidered,
				FinanceAndDebtFurtherSpecification = request.Benefits?.OtherFactorsToConsider?.FinanceAndDebt?.FurtherSpecification,
				OtherRisksShouldBeConsidered = request.Benefits?.OtherFactorsToConsider?.OtherRisks?.ShouldBeConsidered,
				OtherRisksFurtherSpecification = request.Benefits?.OtherFactorsToConsider?.OtherRisks?.FurtherSpecification,
				OtherBenefitValue = request.Benefits?.IntendedTransferBenefits.OtherBenefitValue,
				EqualitiesImpactAssessmentConsidered = request.Benefits?.EqualitiesImpactAssessmentConsidered,
				IncomingTrustAgreement = request.LegalRequirements?.IncomingTrustAgreement,
				DiocesanConsent = request.LegalRequirements?.DiocesanConsent,
				OutgoingTrustConsent = request.LegalRequirements?.OutgoingTrustConsent,
				AcademyTransferProjectIntendedTransferBenefits = ConvertAcademyTransferProjectIntendedTransferBenefits(request.Benefits?.IntendedTransferBenefits?.SelectedBenefits),
				TransferringAcademies = ConvertTransferringAcademiesList(request.TransferringAcademies),
				FeatureSectionIsCompleted = request.Features?.IsCompleted,
				BenefitsSectionIsCompleted = request.Benefits?.IsCompleted,
				LegalRequirementsSectionIsCompleted = request.LegalRequirements?.IsCompleted,
				RationaleSectionIsCompleted = request.Rationale?.IsCompleted,
				HasHtbDate = request.Dates?.HasHtbDate,
				HasTransferFirstDiscussedDate = request.Dates?.HasTransferFirstDiscussedDate,
				HasTargetDateForTransfer = request.Dates?.HasTargetDateForTransfer,
				AssignedUserEmailAddress = request.AssignedUser?.EmailAddress,
				AssignedUserFullName = request.AssignedUser?.FullName,
				AssignedUserId = request.AssignedUser?.Id,
				CreatedOn = DateTimeSource.UtcNow()
			};
		}
	}


}
