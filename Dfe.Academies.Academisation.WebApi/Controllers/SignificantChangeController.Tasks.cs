using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	public partial class SignificantChangeController
	{
		[HttpPut("{id:int}/SetEqualitiesImpactAssessment", Name = "SetSignificantChangeEqualitiesImpactAssessment")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeEqualitiesImpactAssessment(
			int id,
			[FromBody] SetSignificantChangeEqualitiesImpactAssessmentPublicCommand request)
		{
			var command = new SetSignificantChangeEqualitiesImpactAssessmentCommand(
				id,
				request.EqualitiesImpactAssessmentCompleted,
				request.EqualitiesImpactAssessmentNoReason,
				request.EqualitiesImpactIdentified,
				request.EqualitiesLikelyDetails,
				request.EqualitiesSomeImpactDetails,
				request.EqualitiesImpactSupportingEvidence);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetReligiousBodyConsultation", Name = "SetSignificantChangeReligiousBodyConsultation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeReligiousBodyConsultation(
			int id,
			[FromBody] SetSignificantChangeReligiousBodyConsultationPublicCommand request)
		{
			var command = new SetSignificantChangeReligiousBodyConsultationCommand(
				id: id,
				trustConsultedReligiousBody: request.TrustConsultedReligiousBody,
				trustConsultedReligiousBodyNotConsultedReason: request.TrustConsultedReligiousBodyNotConsultedReason,
				religiousBodyRaisedObjection: request.ReligiousBodyRaisedObjection,
				religiousBodyRaisedObjectionReason: request.ReligiousBodyRaisedObjectionReason,
				religiousBodyConsentGranted: request.ReligiousBodyConsentGranted,
				religiousBodyConsentNotGrantedReason: request.ReligiousBodyConsentNotGrantedReason,
				supportingEvidence: request.SupportingEvidence);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetOtherStakeholderEngagement", Name = "SetSignificantChangeOtherStakeholderEngagement")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeOtherStakeholderEngagement(
			int id,
			[FromBody] SetSignificantChangeOtherStakeholderEngagementPublicCommand request)
		{
			var command = new SetSignificantChangeOtherStakeholderEngagementCommand(
				id,
				request.TrustConsultedStakeholders,
				request.TrustConsultedStakeholdersNotConsultedReason,
				request.WereThereAnyObjectionsRaised,
				request.StakeholderObjectionsDetails,
				request.RelevantStakeholdersNotifiedOfAdmissionsChanges,
				request.RelevantStakeholdersNotifiedOfAdmissionsChangesReason,
				request.SupportingEvidence);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetSignificantChangeProjectDates", Name = "SetSignificantChangeProjectDates")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeProjectDates(int id,
			[FromBody] SetSignificantChangeProjectDatesPublicCommand request)
		{
			var command = new SetSignificantChangeProjectDatesCommand(
				id,
				request.ProposedDecisionDate,
				request.ProposedChangeDate);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		[HttpPut("{id:int}/SetConsultationDuration", Name = "SetSignificantChangeConsultationDuration")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeConsultationDuration(
			int id,
			[FromBody] SetSignificantChangeConsultationDurationPublicCommand request)
		{
			var command = new SetSignificantChangeConsultationDurationCommand(
				id: id,
				consultationLastedMinimumThreeWeeks: request.ConsultationLastedMinimumThreeWeeks,
				consultationDurationNotMetReason: request.ConsultationDurationNotMetReason);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetPlanningPermission", Name = "SetSignificantChangePlanningPermission")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangePlanningPermission(
			int id,
			[FromBody] SetSignificantChangePlanningPermissionPublicCommand request)
		{
			var command = new SetSignificantChangePlanningPermissionCommand(id, request.PlanningPermissionAnswer, request.AdditionalInformation, request.SupportingEvidence);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetFunding", Name = "SetSignificantChangeFunding")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeFunding(
			int id,
			[FromBody] SetSignificantChangeFundingPublicCommand request)
		{
			var command = new SetSignificantChangeFundingCommand(
				id,
				request.FundingAnswer,
				request.AdditionalInformation,
				request.SupportingEvidence);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
		
		[HttpPut("{id:int}/SetRecommendation", Name = "SetSignificantChangeRecommendation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeRecommendation(
			int id,
			[FromBody] SetSignificantChangeRecommendationPublicCommand request)
		{
			var command = new SetSignificantChangeRecommendationCommand(
				id,
				request.Recommendation,
				request.RecommendationMoreInformation);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetHQTFConsideration", Name = "SetSignificantChangeHqtfConsideration")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeHqtfConsideration(
			int id,
			[FromBody] SetSignificantChangeHqtfConsiderationPublicCommand request)
		{
			var command = new SetSignificantChangeHqtfConsiderationCommand(
				id,
				request.HqtfConsiderationAnswer,
				request.AdditionalInformation,
				request.SupportingEvidence);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}

		[HttpPut("{id:int}/SetAdmissionsVariation", Name = "SetSignificantChangeAdmissionsVariation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeAdmissionsVariation(
			int id,
			[FromBody] SetSignificantChangeAdmissionVariationPublicCommand request)
		{
			var command = new SetSignificantChangeAdmissionVariationCommand(
				id,
				request.AdmissionsVariationRequired,
				request.AdmissionsVariationSupportingEvidence,
				request.AdmissionsVariationLocalAuthoritySupportive,
				request.AdmissionsVariationPupilPlacePlanningSupportive,
				request.AdmissionsVariationAdmissionsTeamSupportive,
				request.AdmissionsVariationLocalAuthoritySupportiveAdditionalInformation,
				request.AdmissionsVariationPupilPlacePlanningSupportiveAdditionalInformation,
				request.AdmissionsVariationAdmissionsTeamSupportiveAdditionalInformation);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}


		[HttpPut("{id:int}/AdmissionsVariationRecommendation", Name = "SetSignificantChangeAdmissionsVariationRecommendation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeAdmissionsVariationRecommendation(
			int id,
			[FromBody] SetSignificantChangeAdmissionsVariationRecommendationPublicCommand request)
		{
			var command = new SetSignificantChangeAdmissionsVariationRecommendationCommand(
				id,
				request.RecommendationAnswer);

			CommandResult result = await _mediator.Send(command);

			return result switch
			{
				CommandSuccessResult => Ok(),
				NotFoundCommandResult => NotFound(),
				CommandValidationErrorResult validationErrorResult =>
					BadRequest(validationErrorResult.ValidationErrors),
				_ => throw new NotImplementedException()
			};
		}
	}
}
