using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject.SchoolImprovementPlans
{
	public class AddSchoolImprovementPlanCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var Id = 100101;
			var arrangers = new List<SchoolImprovementPlanArranger>() { SchoolImprovementPlanArranger.LocalAuthority };
			var providedBy = "Trust CEO";
			var startDate = DateTime.UtcNow;
			var expectedEndate = SchoolImprovementPlanExpectedEndDate.ToConversion;
			var confidenceLevel = SchoolImprovementPlanConfidenceLevel.Low;
			var planComments = "test comments";

			// Act
			var command = new ConversionProjectAddSchoolImprovementPlanCommand(Id, arrangers, null, providedBy, startDate, expectedEndate, null, confidenceLevel, planComments);


			// Assert
			command.ProjectId.Should().Be(Id);
			command.ArrangedBy.Should().BeEquivalentTo(arrangers);
			command.ArrangedByOther.Should().BeNull();
			command.ProvidedBy.Should().Be(providedBy);
			command.StartDate.Should().Be(startDate);
			command.ExpectedEndDate.Should().Be(expectedEndate);
			command.ExpectedEndDateOther.Should().BeNull();
			command.ConfidenceLevel.Should().Be(confidenceLevel);
			command.PlanComments.Should().Be(planComments);

		}
	}
}
