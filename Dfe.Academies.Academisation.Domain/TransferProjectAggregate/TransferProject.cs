
using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferProject : ITransferProject, IAggregateRoot
	{
		private TransferProject(string outgoingTrustUkprn, string incomingTrustUkprn, List<string> academyUkprns)
		{
			_intendedTransferBenefits =
				new List<IntendedTransferBenefit>();
			_transferringAcademies = new List<TransferringAcademy>();

			OutgoingTrustUkprn = outgoingTrustUkprn;

			foreach (var academyUkprn in academyUkprns)
			{
				_transferringAcademies.Add(new TransferringAcademy(incomingTrustUkprn, academyUkprn));
			}
		}

		protected TransferProject() { }

		public int Id { get; private set; }
		public int Urn { get; private set; }

		public string? ProjectReference { get; private set; }
		public string OutgoingTrustUkprn { get; private set; }
		public string? WhoInitiatedTheTransfer { get; private set; }
		public bool? RddOrEsfaIntervention { get; private set; }
		public string? RddOrEsfaInterventionDetail { get; private set; }
		public string? TypeOfTransfer { get; private set; }
		public string? OtherTransferTypeDescription { get; private set; }
		public DateTime? TransferFirstDiscussed { get; private set; }
		public DateTime? TargetDateForTransfer { get; private set; }
		public DateTime? HtbDate { get; private set; }
		public bool? HasTransferFirstDiscussedDate { get; private set; }
		public bool? HasTargetDateForTransfer { get; private set; }
		public bool? HasHtbDate { get; private set; }
		public string? ProjectRationale { get; private set; }
		public string? TrustSponsorRationale { get; private set; }
		public string? State { get; private set; }
		public string? Status { get; private set; }

		public bool? AnyRisks { get; private set; }
		public bool? HighProfileShouldBeConsidered { get; private set; }
		public string? HighProfileFurtherSpecification { get; private set; }
		public bool? ComplexLandAndBuildingShouldBeConsidered { get; private set; }
		public string? ComplexLandAndBuildingFurtherSpecification { get; private set; }
		public bool? FinanceAndDebtShouldBeConsidered { get; private set; }
		public string? FinanceAndDebtFurtherSpecification { get; private set; }

		public bool? OtherRisksShouldBeConsidered { get; private set; }

		public bool? EqualitiesImpactAssessmentConsidered { get; private set; }

		[MaxLength(20000)]
		public string? OtherRisksFurtherSpecification { get; private set; }
		public string? OtherBenefitValue { get; private set; }
		public string? Author { get; private set; }
		public string? Recommendation { get; private set; }
		public string? IncomingTrustAgreement { get; private set; }
		public string? DiocesanConsent { get; private set; }
		public string? OutgoingTrustConsent { get; private set; }
		public bool? LegalRequirementsSectionIsCompleted { get; private set; }
		public bool? FeatureSectionIsCompleted { get; private set; }
		public bool? BenefitsSectionIsCompleted { get; private set; }
		public bool? RationaleSectionIsCompleted { get; private set; }
		public string? AssignedUserFullName { get; private set; }
		public string? AssignedUserEmailAddress { get; private set; }
		public Guid? AssignedUserId { get; private set; }

		private List<IntendedTransferBenefit> _intendedTransferBenefits;
		public IReadOnlyCollection<IntendedTransferBenefit> IntendedTransferBenefits => _intendedTransferBenefits;

		private List<TransferringAcademy> _transferringAcademies;
		public IReadOnlyCollection<TransferringAcademy> TransferringAcademies => _transferringAcademies;

		IReadOnlyCollection<IIntendedTransferBenefit> ITransferProject.IntendedTransferBenefits => _intendedTransferBenefits;
		IReadOnlyCollection<ITransferringAcademy> ITransferProject.TransferringAcademies => _transferringAcademies;

		public DateTime? CreatedOn { get; private set; }

		public void GenerateUrn(int? urnOverride = null)
		{
			//urn override proably not usefull, but as it is database generated allows us to set it for unit testing the generate logic
			if (urnOverride.HasValue) { Urn = urnOverride.Value; }
			else { Urn = Id; }

			string referenceNumber = "SAT";
			if (TransferringAcademies.Count > 1)
			{
				referenceNumber = "MAT";
			}

			ProjectReference = $"{referenceNumber}-{Urn}";
		}

		public void SetRationale(string projectRationale, string trustSponsorRationale, bool? isCompleted)
		{
			ProjectRationale = projectRationale;
			TrustSponsorRationale = trustSponsorRationale;
			RationaleSectionIsCompleted = isCompleted;
		}

		public void SetFeatures(string whoInitiatedTheTransfer, string transferType, bool? isCompleted)
		{
			TypeOfTransfer = transferType;
			WhoInitiatedTheTransfer = whoInitiatedTheTransfer;
			FeatureSectionIsCompleted = isCompleted;
		}
		public void SetLegalRequirements(string outgoingTrustResolution, string incomingTrustAgreement, string diocesanConsent, bool? isCompleted)
		{
			OutgoingTrustConsent = outgoingTrustResolution;
			IncomingTrustAgreement = incomingTrustAgreement;
			DiocesanConsent = diocesanConsent;
			LegalRequirementsSectionIsCompleted = isCompleted;
		}
		public void SetTransferDates(DateTime advisoryBoardDate, DateTime expectedDateForTransfer)
		{
			// HtbDate maps from the front-end would be good to move this to more business focused language
			HtbDate = advisoryBoardDate;
			TargetDateForTransfer = expectedDateForTransfer;
		}
		public void SetTransferringAcademiesSchoolData(int transferringAcademyId, string latestOfstedReportAdditionalInformation, string pupilNumbersAdditionalInformation, string keyStage2PerformanceAdditionalInformation, string keyStage4PerformanceAdditionalInformation, string keyStage5PerformanceAdditionalInformation)
		{
			var transferringAcademy =
				TransferringAcademies.Single(x => x.Id == transferringAcademyId);



			transferringAcademy.SetSchoolAdditionalData(
				latestOfstedReportAdditionalInformation,
				pupilNumbersAdditionalInformation,
				keyStage2PerformanceAdditionalInformation,
				keyStage4PerformanceAdditionalInformation,
				keyStage5PerformanceAdditionalInformation
			);
		}

		public void SetBenefitsAndRisks(bool? anyRisks, bool? equalitiesImpactAssessmentConsidered, 
			List<string> selectedBenefits, string? otherBenefitValue, 
			bool? highProfileShouldBeConsidered, string? highProfileFurtherSpecification, 
			bool? complexLandAndBuildingShouldBeConsidered, string? complexLandAndBuildingFurtherSpecification, 
			bool? financeAndDebtShouldBeConsidered, string? financeAndDebtFurtherSpecification, 
			bool? otherRisksShouldBeConsidered, string? otherRisksFurtherSpecification, 
			bool? isCompleted)
		{
			AnyRisks = anyRisks;
			EqualitiesImpactAssessmentConsidered = equalitiesImpactAssessmentConsidered;
			BenefitsSectionIsCompleted = isCompleted;

			_intendedTransferBenefits = selectedBenefits.Select(b => new IntendedTransferBenefit(b)).ToList();
			OtherBenefitValue = otherBenefitValue;

			HighProfileShouldBeConsidered = highProfileShouldBeConsidered;
            HighProfileFurtherSpecification = highProfileFurtherSpecification;
			ComplexLandAndBuildingShouldBeConsidered = complexLandAndBuildingShouldBeConsidered;
			ComplexLandAndBuildingFurtherSpecification = complexLandAndBuildingFurtherSpecification;
			FinanceAndDebtShouldBeConsidered = financeAndDebtShouldBeConsidered;
			FinanceAndDebtFurtherSpecification = financeAndDebtFurtherSpecification;
			OtherRisksShouldBeConsidered = otherRisksShouldBeConsidered;
			OtherRisksFurtherSpecification = otherRisksFurtherSpecification;
		}

		public static TransferProject Create(string outgoingTrustUkprn, string incomingTrustUkprn, List<string> academyUkprns, DateTime createdOn)
		{
			Guard.Against.NullOrEmpty(outgoingTrustUkprn);
			Guard.Against.NullOrEmpty(incomingTrustUkprn);
			Guard.Against.NullOrEmpty(academyUkprns);
			Guard.Against.OutOfSQLDateRange(createdOn);

			return new TransferProject(outgoingTrustUkprn, incomingTrustUkprn, academyUkprns)
			{
				CreatedOn = createdOn
			};
		}
	}



}
