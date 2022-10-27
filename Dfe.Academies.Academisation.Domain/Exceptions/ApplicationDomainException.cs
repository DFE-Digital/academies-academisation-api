namespace Dfe.Academies.Academisation.Domain.Exceptions;

public class ApplicationDomainException : Exception
{
	public ApplicationDomainException() { }
	public ApplicationDomainException(string message) : base(message) { }
	public ApplicationDomainException(string message, Exception innerException) : base(message, innerException) { }
}
