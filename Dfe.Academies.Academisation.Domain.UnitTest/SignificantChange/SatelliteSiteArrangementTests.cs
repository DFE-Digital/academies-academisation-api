using System;
using Dfe.Academies.Academisation.Domain.SignificantChange;
using Xunit;

namespace Dfe.Academies.Academisation.Domain.UnitTest.SignificantChange;

public class SatelliteSiteArrangementTests
{
	[Theory]
	[InlineData(YesNoNotApplicableAnswer.No, 2, "Mitigation")]
	[InlineData(YesNoNotApplicableAnswer.Yes, 1, null)]
	[InlineData(YesNoNotApplicableAnswer.NotApplicable, 1, null)]
	[InlineData(null, 1, null)]
	public void SetSatelliteSiteArrangement_StoresAnswerAndEvidenceAndRecalculatesTier(
		YesNoNotApplicableAnswer? answer, byte expectedTier, string? expectedInformation)
	{
		var project = CreateProject();

		project.SetSatelliteSiteArrangement(answer, "Mitigation", "Evidence");

		Assert.Equal(answer, project.Details.SatelliteSiteArrangementAnswer);
		Assert.Equal(expectedInformation, project.Details.SatelliteSiteArrangementAdditionalInformation);
		Assert.Equal("Evidence", project.Details.SatelliteSiteArrangementSupportingEvidence);
		Assert.Equal(expectedTier, project.Tier);
	}

	[Theory]
	[InlineData(YesNoNotApplicableAnswer.Yes)]
	[InlineData(YesNoNotApplicableAnswer.NotApplicable)]
	public void ChangingNoToNonRiskAnswer_ClearsInformationAndRemovesTierTwoTrigger(YesNoNotApplicableAnswer answer)
	{
		var project = CreateProject();
		project.SetSatelliteSiteArrangement(YesNoNotApplicableAnswer.No, "Mitigation", null);

		project.SetSatelliteSiteArrangement(answer, "Stale information", null);

		Assert.Equal(1, project.Tier);
		Assert.Null(project.Details.SatelliteSiteArrangementAdditionalInformation);
	}

	[Fact]
	public void ChangingAnswer_DoesNotRemoveAnotherTierTwoTrigger()
	{
		var project = CreateProject();
		project.SetFunding(FundingAnswer.No, "Funding gap", null);
		project.SetSatelliteSiteArrangement(YesNoNotApplicableAnswer.No, "Mitigation", null);

		project.SetSatelliteSiteArrangement(YesNoNotApplicableAnswer.Yes, null, null);

		Assert.Equal(2, project.Tier);
	}

	[Fact]
	public void SetSatelliteSiteArrangement_PreservesTierThree()
	{
		var project = CreateProject(3);

		project.SetSatelliteSiteArrangement(YesNoNotApplicableAnswer.No, "Mitigation", null);

		Assert.Equal(3, project.Tier);
	}

	[Theory]
	[InlineData(null, null, null, SignificantChangeTaskStatus.NotStarted)]
	[InlineData(null, null, "Evidence", SignificantChangeTaskStatus.InProgress)]
	[InlineData(null, "Information", null, SignificantChangeTaskStatus.InProgress)]
	[InlineData(YesNoNotApplicableAnswer.No, null, null, SignificantChangeTaskStatus.InProgress)]
	[InlineData(YesNoNotApplicableAnswer.No, " ", "Evidence", SignificantChangeTaskStatus.InProgress)]
	[InlineData(YesNoNotApplicableAnswer.No, "Mitigation", null, SignificantChangeTaskStatus.Completed)]
	[InlineData(YesNoNotApplicableAnswer.Yes, null, null, SignificantChangeTaskStatus.Completed)]
	[InlineData(YesNoNotApplicableAnswer.NotApplicable, null, null, SignificantChangeTaskStatus.Completed)]
	public void TaskStatus_RequiresAnAnswerAndInformationForNo(
		YesNoNotApplicableAnswer? answer, string? information, string? evidence, SignificantChangeTaskStatus expected)
	{
		var details = new SignificantChangeProjectDetails
		{
			SatelliteSiteArrangementAnswer = answer,
			SatelliteSiteArrangementAdditionalInformation = information,
			SatelliteSiteArrangementSupportingEvidence = evidence
		};

		Assert.Equal(expected, details.GetSatelliteSiteArrangementTaskStatus());
	}

	private static SignificantChangeProject CreateProject(byte tier = 1) => SignificantChangeProject.Create(
		new SignificantChangeProjectOptions(123456, tier, "Test Trust", "12345678", "Change of age range", "Test School"),
		DateTime.UtcNow);
}
