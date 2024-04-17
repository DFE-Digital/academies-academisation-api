using AutoFixture;
using FluentAssertions;
using TramsDataApi.RequestModels.AcademyTransferProject;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.TransferProject
{
	public class SetTransferProjectBenefitsCommandTests
    {
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			Fixture fixture = new Fixture();

			// Arrange
			var urn = fixture.Create<int>();
			var anyRisks = fixture.Create<bool>();
			bool? equalitiesImpactAssessmentConsidered = fixture.Create<bool>();
			List<string> selectedBenefits = fixture.Create<List<string>>();
			string otherBenefitValue = fixture.Create<string>();
			bool highProfileShouldBeConsidered = fixture.Create<bool>();
			string highProfileFurtherSpecification = fixture.Create<string>();
			bool complexLandAndBuildingShouldBeConsidered = fixture.Create<bool>();
			string complexLandAndBuildingFurtherSpecification = fixture.Create<string>();
			bool financeAndDebtShouldBeConsidered = fixture.Create<bool>();
			string financeAndDebtFurtherSpecification = fixture.Create<string>();
			bool otherRisksShouldBeConsidered = fixture.Create<bool>();
			string otherRisksFurtherSpecification = fixture.Create<string>();
			bool isCompleted = fixture.Create<bool>();

			// Act
			var command = new SetTransferProjectBenefitsCommand
			{
				Urn = urn,
				AnyRisks = anyRisks,
				EqualitiesImpactAssessmentConsidered = equalitiesImpactAssessmentConsidered,
				IsCompleted = isCompleted,
				IntendedTransferBenefits = new IntendedTransferBenefitDto() { OtherBenefitValue = otherBenefitValue, SelectedBenefits = selectedBenefits },
				OtherFactorsToConsider = new OtherFactorsToConsiderDto() { 
					ComplexLandAndBuilding = new BenefitConsideredFactorDto() { FurtherSpecification = complexLandAndBuildingFurtherSpecification, ShouldBeConsidered = complexLandAndBuildingShouldBeConsidered }, 
					FinanceAndDebt = new BenefitConsideredFactorDto() { FurtherSpecification = financeAndDebtFurtherSpecification, ShouldBeConsidered = financeAndDebtShouldBeConsidered },
					HighProfile = new BenefitConsideredFactorDto() { FurtherSpecification = highProfileFurtherSpecification, ShouldBeConsidered = highProfileShouldBeConsidered },
					OtherRisks = new BenefitConsideredFactorDto() { FurtherSpecification = otherRisksFurtherSpecification, ShouldBeConsidered = otherRisksShouldBeConsidered },
				}
			};

			// Assert
			command.AnyRisks.Should().Be(anyRisks);
			command.EqualitiesImpactAssessmentConsidered.Should().Be(equalitiesImpactAssessmentConsidered);
			command.IntendedTransferBenefits.SelectedBenefits.All(x => selectedBenefits.Contains(x)).Should().BeTrue();
			command.IntendedTransferBenefits.OtherBenefitValue.Should().Be(otherBenefitValue);
			command.OtherFactorsToConsider.HighProfile.ShouldBeConsidered.Should().Be(highProfileShouldBeConsidered);
			command.OtherFactorsToConsider.HighProfile.FurtherSpecification.Should().Be(highProfileFurtherSpecification);
			command.OtherFactorsToConsider.ComplexLandAndBuilding.ShouldBeConsidered.Should().Be(complexLandAndBuildingShouldBeConsidered);
			command.OtherFactorsToConsider.ComplexLandAndBuilding.FurtherSpecification.Should().Be(complexLandAndBuildingFurtherSpecification);
			command.OtherFactorsToConsider.FinanceAndDebt.ShouldBeConsidered.Should().Be(financeAndDebtShouldBeConsidered);
			command.OtherFactorsToConsider.FinanceAndDebt.FurtherSpecification.Should().Be(financeAndDebtFurtherSpecification);
			command.OtherFactorsToConsider.OtherRisks.ShouldBeConsidered.Should().Be(otherRisksShouldBeConsidered);
			command.OtherFactorsToConsider.OtherRisks.FurtherSpecification.Should().Be(otherRisksFurtherSpecification);
			command.IsCompleted.Should().Be(isCompleted);
		}
	}
}
