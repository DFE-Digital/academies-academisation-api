﻿
namespace Dfe.Academies.Academisation.IDomain.TransferProjectAggregate

{
	public interface ITransferProject
	{
		bool? AnyRisks { get; }
		string? AssignedUserEmailAddress { get; }
		string? AssignedUserFullName { get; }
		Guid? AssignedUserId { get; }
		string? Author { get; }
		bool? BenefitsSectionIsCompleted { get; }
		string? ComplexLandAndBuildingFurtherSpecification { get; }
		bool? ComplexLandAndBuildingShouldBeConsidered { get; }
		DateTime? CreatedOn { get; }
		string? DiocesanConsent { get; }
		bool? EqualitiesImpactAssessmentConsidered { get; }
		bool? FeatureSectionIsCompleted { get; }
		string? FinanceAndDebtFurtherSpecification { get; }
		bool? FinanceAndDebtShouldBeConsidered { get; }
		bool? HasHtbDate { get; }
		bool? HasTargetDateForTransfer { get; }
		bool? HasTransferFirstDiscussedDate { get; }
		string? HighProfileFurtherSpecification { get; }
		bool? HighProfileShouldBeConsidered { get; }
		DateTime? HtbDate { get; }
		int Id { get; }
		string? IncomingTrustAgreement { get; }
		IReadOnlyCollection<IIntendedTransferBenefit> IntendedTransferBenefits { get; }
		bool? LegalRequirementsSectionIsCompleted { get; }
		string? OtherBenefitValue { get; }
		string? OtherRisksFurtherSpecification { get; }
		bool? OtherRisksShouldBeConsidered { get; }
		string? OtherTransferTypeDescription { get; }
		string? OutgoingTrustConsent { get; }
		string OutgoingTrustUkprn { get; }
		string? ProjectRationale { get; }
		string? ProjectReference { get; }
		bool? RationaleSectionIsCompleted { get; }
		bool? RddOrEsfaIntervention { get; }
		string? RddOrEsfaInterventionDetail { get; }
		string? Recommendation { get; }
		string? State { get; }
		string? Status { get; }
		DateTime? TargetDateForTransfer { get; }
		DateTime? TransferFirstDiscussed { get; }
		IReadOnlyCollection<ITransferringAcademy> TransferringAcademies { get; }
		string? TrustSponsorRationale { get; }
		string? TypeOfTransfer { get; }
		int Urn { get; }
		string? WhoInitiatedTheTransfer { get; }

		void GenerateUrn(int? urnOverride = null);
		void SetRationale(string projectRationale, string trustSponsorRationale, bool? isCompleted);
		void AssignUser(Guid userId, string userEmail, string userFullName);
		void SetFeatures(string whoInitiatedTheTransfer, string transferType, bool? isCompleted);
		void SetLegalRequirements(string outgoingTrustResolution, string incomingTrustAgreement,
			string diocesanConsent, bool? isCompleted);

		void SetTransferDates(DateTime advisoryBoardDate, DateTime expectedDateForTransfer);

		void SetTransferringAcademiesSchoolData(int transferringAcademyId,
			string latestOfstedReportAdditionalInformation, string pupilNumbersAdditionalInformation,
			string keyStage2PerformanceAdditionalInformation, string keyStage4PerformanceAdditionalInformation,
			string keyStage5PerformanceAdditionalInformation);

		void SetBenefitsAndRisks(bool? anyRisks, bool? equalitiesImpactAssessmentConsidered,
			List<string> selectedBenefits, string? otherBenefitValue,
			bool? highProfileShouldBeConsidered, string? highProfileFurtherSpecification,
			bool? complexLandAndBuildingShouldBeConsidered, string? complexLandAndBuildingFurtherSpecification,
			bool? financeAndDebtShouldBeConsidered, string? financeAndDebtFurtherSpecification,
			bool? otherRisksShouldBeConsidered, string? otherRisksFurtherSpecification,
			bool? isCompleted);

		void SetTrustInformationAndProjectDates(string recommendation, string author);
	}
}