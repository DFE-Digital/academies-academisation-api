using Dfe.Academies.Academisation.Core;
using MediatR;

namespace TramsDataApi.RequestModels.AcademyTransferProject
{
    public class SetTransferProjectBenefitsCommand : IRequest<CommandResult>
	{
		public int Id { get; set; }
        public IntendedTransferBenefitDto IntendedTransferBenefits { get; set; }
        public OtherFactorsToConsiderDto OtherFactorsToConsider { get; set; }
        public bool? EqualitiesImpactAssessmentConsidered { get; set; }
        public bool? AnyRisks { get; set; }
        public bool? IsCompleted { get; set; }

    }

	public class IntendedTransferBenefitDto
	{
		public List<string> SelectedBenefits { get; set; }
		public string OtherBenefitValue { get; set; }
	}

	public class OtherFactorsToConsiderDto
	{
		public BenefitConsideredFactorDto HighProfile { get; set; }
		public BenefitConsideredFactorDto ComplexLandAndBuilding { get; set; }
		public BenefitConsideredFactorDto FinanceAndDebt { get; set; }
		public BenefitConsideredFactorDto OtherRisks { get; set; }
	}

	public class BenefitConsideredFactorDto
	{
		public bool ShouldBeConsidered { get; set; }
		public string FurtherSpecification { get; set; }
	}
}
