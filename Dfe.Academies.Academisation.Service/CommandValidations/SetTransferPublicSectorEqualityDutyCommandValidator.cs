using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations
{
	public class SetTransferPublicSectorEqualityDutyCommandValidator : AbstractValidator<SetTransferPublicSectorEqualityDutyCommand>
	{
		public SetTransferPublicSectorEqualityDutyCommandValidator()
		{
			RuleFor(x => x.HowLikelyImpactProtectedCharacteristics)
				.NotNull()
				.WithMessage("How Likely Impact Protected Characteristics must not be null");
		}
	}
}
