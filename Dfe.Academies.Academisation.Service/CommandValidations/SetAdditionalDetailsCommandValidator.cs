using System.Data;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations;

public class SetAdditionalDetailsCommandValidator : AbstractValidator<SetAdditionalDetailsCommand>
{
	public SetAdditionalDetailsCommandValidator()
	{
		RuleFor(x => x.TrustBenefitDetails)
			.NotEmpty()
			.WithMessage("Trust Benefit Details must not be empty");
		RuleFor(x => x.OfstedInspectionDetails)
			.NotEmpty()
			.When(x => x.OfstedInspectionDetails != null)
			.WithMessage("Ofsted Inspection Details must not be empty");
		RuleFor(x => x.SafeguardingDetails)
			.NotEmpty()
			.When(x => x.SafeguardingDetails != null)
			.WithMessage("Safeguarding Details must not be empty");
		RuleFor(x => x.LocalAuthorityReorganisationDetails)
			.NotEmpty()
			.When(x => x.LocalAuthorityReorganisationDetails != null)
			.WithMessage("Local Authority Reorganisation Details must not be empty");
		RuleFor(x => x.LocalAuthorityClosurePlanDetails)
			.NotEmpty()
			.When(x => x.LocalAuthorityClosurePlanDetails != null)
			.WithMessage("Local Authority Closure Plan Details must not be empty");
		RuleFor(x => x.DioceseName)
			.NotEmpty()
			.When(x => x.DioceseName != null)
			.WithMessage("Diocese Name must not be empty");
		RuleFor(x => x.DioceseFolderIdentifier)
			.NotEmpty()
			.When(x => x.DioceseName != null)
			.WithMessage("You must upload a file when specifying a Diocese Name");
		RuleFor(x => x.FoundationTrustOrBodyName)
			.NotEmpty()
			.When(x => x.FoundationTrustOrBodyName != null)
			.WithMessage("Foundation Trust Or Body Name must not be empty");
		RuleFor(x => x.FoundationConsentFolderIdentifier)
			.NotEmpty()
			.When(x => x.FoundationTrustOrBodyName != null)
			.WithMessage("You must upload a file when the Foundation, Trust or Governing body is not empty");
		RuleFor(x => x.ExemptionEndDate)
			.GreaterThan(DateTimeOffset.UtcNow)
			.WithMessage("Exemption End Date must be in the future");
		RuleFor(x => x.MainFeederSchools)
			.NotEmpty()
			.WithMessage("Main Feeder Schools must not be empty");
		RuleFor(x => x.ResolutionConsentFolderIdentifier)
			.NotEmpty()
			.WithMessage("Resolution Consent must have a file");
		RuleFor(x => x.ProtectedCharacteristics)
			.IsInEnum()
			.When(x => x.ProtectedCharacteristics != null)
			.WithMessage("Protected Characteristics must contain a valid value");
		RuleFor(x => x.FurtherInformation)
			.NotEmpty()
			.When(x => x.FurtherInformation != null)
			.WithMessage("Further information must not be empty");
	}
}
