namespace Dfe.Academies.Academisation.Core;

public record ValidationError
(
	string PropertyName,
	string ErrorMessage
);
