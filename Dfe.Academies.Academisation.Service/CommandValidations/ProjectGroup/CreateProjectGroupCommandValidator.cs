﻿using Dfe.Academies.Academisation.Service.Commands.ProjectGroup;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations.ProjectGroup
{
	public class CreateProjectGroupCommandValidator : AbstractValidator<CreateProjectGroupCommand>
	{
		public CreateProjectGroupCommandValidator()
		{
			RuleFor(x => x.TrustReference)
				.NotEmpty().WithMessage("Must specify a trust reference")
				.Length(8).WithMessage("Trust reference must be length 8")
				.NotNull().WithMessage("Trust Reference must not be null");

			RuleFor(x => x.ReferenceNumber)
				.NotEmpty().WithMessage("Must specify a reference number")
				.Length(8).WithMessage("Reference number must be length 8")
				.NotNull().WithMessage("Reference must not be null");
		}
	}
}
