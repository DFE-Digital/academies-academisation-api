﻿
using System.ComponentModel.DataAnnotations;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.Exceptions;
using Dfe.Academies.Academisation.Domain.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.TransferProjectAggregate
{
	public class TransferProject : IAggregateRoot
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

		private readonly List<IntendedTransferBenefit> _intendedTransferBenefits;
		public IReadOnlyCollection<IntendedTransferBenefit> IntendedTransferBenefits => _intendedTransferBenefits;

		private readonly List<TransferringAcademy> _transferringAcademies;
		public IReadOnlyCollection<TransferringAcademy> TransferringAcademies => _transferringAcademies;

		public DateTime? CreatedOn { get; private set; }

		public void GenerateUrn() {
			string referenceNumber = "SAT";
			if (TransferringAcademies.Count > 1)
			{
				referenceNumber = "MAT";
			}

			ProjectReference = $"{referenceNumber}-{Urn}";
		}

		public static TransferProject Create(string outgoingTrustUkprn, string incomingTrustUkprn, List<string> academyUkprns, DateTime createdOn)
		{
			return new TransferProject(outgoingTrustUkprn, incomingTrustUkprn, academyUkprns)
			{
				CreatedOn = createdOn
			};
		}
	}



}
