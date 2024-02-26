namespace Dfe.Academies.Academisation.Service.Extensions
{
	public static class ListExtensions
	{

		public static string JoinNonEmpty(this IEnumerable<string?> source, string separator)
		{
			return string.Join(separator, source.Where(s => !string.IsNullOrWhiteSpace(s)));
		}
	}
}
