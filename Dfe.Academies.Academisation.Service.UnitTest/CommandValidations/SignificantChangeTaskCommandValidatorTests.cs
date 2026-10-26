using Dfe.Academies.Academisation.Domain.SignificantChange;
using Dfe.Academies.Academisation.Service.CommandValidations.SignificantChange;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using FluentValidation.TestHelper;
using Xunit;

namespace Dfe.Academies.Academisation.Service.UnitTest.CommandValidations;

public class SignificantChangeTaskCommandValidatorTests
{
	[Fact]
	public void ConsultationDurationAnswerValuesRemainStable()
	{
		Assert.Equal(1, (int)ConsultationDurationAnswer.Yes);
		Assert.Equal(2, (int)ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut);
		Assert.Equal(3, (int)ConsultationDurationAnswer.No);
	}

	[Fact]
	public void ConsultationDuration_RequiresReasonOnlyWhenAnswerIsNo()
	{
		var validator = new SetSignificantChangeConsultationDurationCommandValidator();
		var noAnswer = new SetSignificantChangeConsultationDurationCommand(1, ConsultationDurationAnswer.No, null);
		var noSatisfactoryConsultation = new SetSignificantChangeConsultationDurationCommand(
			1, ConsultationDurationAnswer.NoSatisfactoryConsultationCarriedOut, null);

		validator.TestValidate(noAnswer)
			.ShouldHaveValidationErrorFor(command => command.ConsultationDurationNotMetReason);
		validator.TestValidate(noSatisfactoryConsultation).ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void ReligiousBodyAnswers_RequireDetailsOnlyForNoAnswers()
	{
		var validator = new SetSignificantChangeReligiousBodyConsultationCommandValidator();
		var noAnswersWithoutDetails = new SetSignificantChangeReligiousBodyConsultationCommand(
			1,
			YesNoNotApplicableAnswer.No,
			null,
			YesNoNotApplicableAnswer.No,
			null,
			YesNoNotApplicableAnswer.No,
			null,
			null);
		var nonNoAnswersWithoutDetails = new SetSignificantChangeReligiousBodyConsultationCommand(
			1,
			YesNoNotApplicableAnswer.Yes,
			null,
			YesNoNotApplicableAnswer.NotApplicable,
			null,
			YesNoNotApplicableAnswer.Yes,
			null,
			null);

		var errors = validator.TestValidate(noAnswersWithoutDetails);
		errors.ShouldHaveValidationErrorFor(command => command.TrustConsultedReligiousBodyNotConsultedReason);
		errors.ShouldHaveValidationErrorFor(command => command.ReligiousBodyRaisedObjectionReason);
		errors.ShouldHaveValidationErrorFor(command => command.ReligiousBodyConsentNotGrantedReason);
		validator.TestValidate(nonNoAnswersWithoutDetails).ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void OtherStakeholderAnswers_RequireOnlyConditionallyDisplayedDetails()
	{
		var validator = new SetSignificantChangeOtherStakeholderEngagementCommandValidator();
		var validWithoutConditionalDetails = new SetSignificantChangeOtherStakeholderEngagementCommand(
			1, true, null, false, null, YesNoNotApplicableAnswer.NotApplicable, null, null);
		var missingConditionalDetails = new SetSignificantChangeOtherStakeholderEngagementCommand(
			1, false, null, true, null, YesNoNotApplicableAnswer.No, null, null);

		validator.TestValidate(validWithoutConditionalDetails).ShouldNotHaveAnyValidationErrors();

		var errors = validator.TestValidate(missingConditionalDetails);
		errors.ShouldHaveValidationErrorFor(command => command.TrustConsultedStakeholdersNotConsultedReason);
		errors.ShouldHaveValidationErrorFor(command => command.StakeholderObjectionsDetails);
		errors.ShouldHaveValidationErrorFor(command => command.RelevantStakeholdersNotifiedOfAdmissionsChangesReason);
	}
}
