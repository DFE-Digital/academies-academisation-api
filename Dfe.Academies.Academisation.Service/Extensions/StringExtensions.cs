namespace Dfe.Academies.Academisation.Service.Extensions;
 
public static class StringExtensions
{
	public static (string? firstName, string? lastName) GetFirstAndLastName(this string? fullName)
	{
		if (string.IsNullOrWhiteSpace(fullName))
			return (null, null);

		string[] parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		string? firstName = parts.Length > 0 ? parts[0] : null;
		string? lastName = parts.Length > 1 ? parts[1] : null;

		return (firstName, lastName);
	}
}
