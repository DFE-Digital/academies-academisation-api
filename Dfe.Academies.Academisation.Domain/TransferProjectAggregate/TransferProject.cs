using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.TransferProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferProject : ITransferProject, IAggregateRoot
	{
		private TransferProject(string outgoingTrustUkprn, string outgoingTrustName, string? incomingTrustUkprn, string? incomingTrustName, List<string> academyUkprns, bool? isFormAMat)
		{
			_intendedTransferBenefits = new List<IntendedTransferBenefit>();
			_transferringAcademies = new List<TransferringAcademy>();
			_specificReasonsForTransfer = new List<string>();

			OutgoingTrustUkprn = outgoingTrustUkprn;
			OutgoingTrustName = outgoingTrustName;
			IsFormAMat = isFormAMat.HasValue && isFormAMat.Value;

			foreach (var academyUkprn in academyUkprns)
			{
				_transferringAcademies.Add(new TransferringAcademy(incomingTrustUkprn, incomingTrustName, academyUkprn));
			}
		}

		protected TransferProject() { }

		public int Id { get; private set; }
		public int Urn { get; private set; }
		public string? ProjectReference { get; private set; }
		public string OutgoingTrustUkprn { get; private set; }
		public string? OutgoingTrustName { get; private set; }
		public string? WhoInitiatedTheTransfer { get; private set; }

		private List<string> _specificReasonsForTransfer;
		public IReadOnlyCollection<string> SpecificReasonsForTransfer => _specificReasonsForTransfer;
		IReadOnlyCollection<string> ITransferProject.SpecificReasonsForTransfer => _specificReasonsForTransfer;

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
		public bool? IsFormAMat { get; private set; }

		private List<IntendedTransferBenefit> _intendedTransferBenefits;
		public IReadOnlyCollection<IntendedTransferBenefit> IntendedTransferBenefits => _intendedTransferBenefits;

		private List<TransferringAcademy> _transferringAcademies;
		public IReadOnlyCollection<TransferringAcademy> TransferringAcademies => _transferringAcademies;
		IReadOnlyCollection<IIntendedTransferBenefit> ITransferProject.IntendedTransferBenefits => _intendedTransferBenefits;
		IReadOnlyCollection<ITransferringAcademy> ITransferProject.TransferringAcademies => _transferringAcademies;

		public DateTime? CreatedOn { get; private set; }

		public void GenerateUrn(int? urnOverride = null)
		{
			if (urnOverride.HasValue)
			{
				Urn = urnOverride.Value;
			}
			else
			{
				Urn = Id;
			}

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

		public void SetGeneralInformation(string recommendation, string author)
		{
			Recommendation = recommendation;
			Author = author;
		}

		public void AssignUser(Guid userId, string userEmail, string userFullName)
		{
			AssignedUserId = userId;
			AssignedUserEmailAddress = userEmail;
			AssignedUserFullName = userFullName;
		}

		public void SetFeatures(string whoInitiatedTheTransfer, List<string> specificReasonsForTransfer, string transferType, bool? isCompleted)
		{
			WhoInitiatedTheTransfer = whoInitiatedTheTransfer;
			_specificReasonsForTransfer = specificReasonsForTransfer;
			TypeOfTransfer = transferType;
			FeatureSectionIsCompleted = isCompleted;
		}

		public void SetLegalRequirements(string outgoingTrustResolution, string incomingTrustAgreement, string diocesanConsent, bool? isCompleted)
		{
			OutgoingTrustConsent = outgoingTrustResolution;
			IncomingTrustAgreement = incomingTrustAgreement;
			DiocesanConsent = diocesanConsent;
			LegalRequirementsSectionIsCompleted = isCompleted;
		}

		public void SetTransferDates(DateTime? advisoryBoardDate, DateTime? expectedDateForTransfer)
		{
			HtbDate = advisoryBoardDate;
			TargetDateForTransfer = expectedDateForTransfer;
		}

		public void SetTransferringAcademiesSchoolData(string transferringAcademyUkprn, string latestOfstedReportAdditionalInformation, string pupilNumbersAdditionalInformation, string keyStage2PerformanceAdditionalInformation, string keyStage4PerformanceAdditionalInformation, string keyStage5PerformanceAdditionalInformation)
		{
			var transferringAcademy = TransferringAcademies.Single(x => x.OutgoingAcademyUkprn == transferringAcademyUkprn);

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

		public void SetStatus(string status)
		{
			Status = status;
		}

		public static TransferProject Create(string outgoingTrustUkprn, string outgoingTrustName, string? incomingTrustUkprn, string? incomingTrustName, List<string> academyUkprns, bool? isFormAMat, DateTime createdOn)
		{
			Guard.Against.NullOrEmpty(outgoingTrustUkprn);
			Guard.Against.NullOrEmpty(outgoingTrustName);
			Guard.Against.NullOrEmpty(academyUkprns);
			Guard.Against.OutOfSQLDateRange(createdOn);

			return new TransferProject(outgoingTrustUkprn, outgoingTrustName, incomingTrustUkprn, incomingTrustName, academyUkprns, isFormAMat)
			{
				CreatedOn = createdOn
			};
		}

		public void SetOutgoingTrustName(string outgoingTrustName)
		{
			OutgoingTrustName = outgoingTrustName;
		}

		public void SetAcademyIncomingTrust(int academyId, string incomingTrustName, string? incomingTrustUKPRN)
		{
			var transferringAcademy = TransferringAcademies.SingleOrDefault(x => x.Id == academyId);

			if (transferringAcademy != null)
			{
				transferringAcademy.SetIncomingTrustName(incomingTrustName, incomingTrustUKPRN);
			}
		}

		public void SetTransferringAcademyGeneralInformation(string transferringAcademyUkprn, string pfiScheme, string pfiSchemeDetails)
		{
			var transferringAcademy = TransferringAcademies.Single(x => x.OutgoingAcademyUkprn == transferringAcademyUkprn);

			transferringAcademy.SetGeneralInformation(pfiScheme, pfiSchemeDetails);
		}
	}
}
