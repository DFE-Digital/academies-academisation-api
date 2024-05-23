
using Dfe.Academies.Academisation.Service.Commands.TransferProject;
using FluentValidation;

namespace Dfe.Academies.Academisation.Service.CommandValidations
{
	public class CreateTransferProjectCommandValidator : AbstractValidator<CreateTransferProjectCommand>
	{
		public CreateTransferProjectCommandValidator()
		{
			RuleFor(x => x.OutgoingTrustUkprn).Length(8)
				.WithMessage("OutgoingTrustUkprn must be length 8")
				.NotNull().WithMessage("OutgoingTrustUkprn must not be null");

			RuleForEach(x => x.TransferringAcademies).ChildRules(ta =>
			{
				ta.RuleFor(x => x.OutgoingAcademyUkprn).Length(8)
				.WithMessage("OutgoingAcademyUkprn must be length 8")
				.NotNull().WithMessage("OutgoingAcademyUkprn must not be null");
			});
		}
	}
}
