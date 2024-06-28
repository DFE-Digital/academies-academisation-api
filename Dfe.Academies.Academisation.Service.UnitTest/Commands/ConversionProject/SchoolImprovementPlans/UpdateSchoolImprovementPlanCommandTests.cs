using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate.SchoolImprovemenPlans;
using Dfe.Academies.Academisation.Service.Commands.ConversionProject.SchoolImprovementPlan;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.Commands.ConversionProject.SchoolImprovementPlans
{
	public class UpdateSchoolImprovementPlanCommandTests
	{
		[Fact]
		public void CommandProperties_AreSetCorrectly()
		{
			// Arrange
			var id = 10;
			var projectId = 100101;
			var arrangers = new List<SchoolImprovementPlanArranger>() { SchoolImprovementPlanArranger.LocalAuthority };
			var providedBy = "Trust CEO";
			var startDate = DateTime.UtcNow;
			var expectedEndate = SchoolImprovementPlanExpectedEndDate.ToConversion;
			var confidenceLevel = SchoolImprovementPlanConfidenceLevel.Low;
			var planComments = "test comments";

			// Act
			var command = new ConversionProjectUpdateSchoolImprovementPlanCommand(id, projectId, arrangers, null, providedBy, startDate, expectedEndate, null, confidenceLevel, planComments);


			// Assert
			command.Id.Should().Be(id);
			command.ProjectId.Should().Be(projectId);
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
