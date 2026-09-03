using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Service.Commands.SignificantChange;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Academies.Academisation.WebApi.Controllers
{
	public partial class SignificantChangeController
	{
		[HttpPut("{id:int}/SetStakeholderConsultation", Name = "SetSignificantChangeStakeholderConsultation")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> SetSignificantChangeStakeholderConsultation(
			int id,
			[FromBody] SetSignificantChangeStakeholderConsultationPublicCommand request)
		{
			var command = new SetSignificantChangeStakeholderConsultationCommand(
				id: id,
				trustConsultedStakeholders: request.TrustConsultedStakeholders,
				trustConsultedStakeholdersNotConsultedReason: request.TrustConsultedStakeholdersNotConsultedReason);

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
                request.EqualitiesImpactIdentified,
                request.EqualitiesImpactIdentifiedMitigation);

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
