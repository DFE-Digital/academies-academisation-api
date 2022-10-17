using System.Reflection;
using Dfe.Academies.Academisation.IService.ServiceModels.Application.School;
using FluentValidation;
using MediatR;

namespace Dfe.Academies.Academisation.Service.CommandValidations
{
	public interface IValidatorFactory<T> where T : IRequest<bool>
	{
		AbstractValidator<T> GetCommandValidator();
	}

	public class ValidatorFactory<T> : IValidatorFactory<T> where T : IRequest<bool>
	{
		private readonly IEnumerable<AbstractValidator<T>> _commandValidators;

		public ValidatorFactory(IEnumerable<AbstractValidator<T>> commandValidators)
		{
			_commandValidators = commandValidators;
		}

		public AbstractValidator<T> GetCommandValidator()
		{
			return _commandValidators.First(x => x.GetType().BaseType?.GetGenericArguments().First() == typeof(T));
		}
	}
}
