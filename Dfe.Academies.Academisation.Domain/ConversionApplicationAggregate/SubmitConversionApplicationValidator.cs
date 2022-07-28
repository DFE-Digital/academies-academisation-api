using Dfe.Academies.Academisation.Domain.Core.ConversionApplicationAggregate;
using FluentValidation;

namespace Dfe.Academies.Academisation.Domain.ConversionApplicationAggregate
{
	internal class SubmitConversionApplicationValidator : AbstractValidator<ConversionApplication>
	{
		public SubmitConversionApplicationValidator()
		{
			RuleFor(application => application.ApplicationStatus)
				.Equal(ApplicationStatus.InProgress);
		}
	}
}
