using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Dfe.Academies.Academisation.SubcutaneousTest.Utils
{
	internal static class Request
	{
		internal static StringContent ConvertRequestObjectToContent(object request) => new(JsonSerializer.Serialize(request), Encoding.Default, "application/json");

		internal static string ComplexTypeToQueryString(object obj)
		{
			var encode = obj.GetType().GetProperties()
				.Where(p => p.GetValue(obj) != null)
				.Select(p => $"{ToCamelCase(p.Name)}={HttpUtility.UrlEncode(ToString(p.GetValue(obj)!))}");

			return string.Join("&", encode);
		}

		internal static string NestedComplexTypeToQueryString(this object obj, string prefix = "")
		{
			var properties = obj.GetType().GetProperties()
				.Where(p => p.GetValue(obj) != null);

			return string.Join('&',
				properties.Select(prop =>
				{
					var name = HttpUtility.UrlEncode(ToCamelCase(prop.Name));
					if (prop.GetValue(obj) is System.Collections.IEnumerable enumerable)
					{
						return string.Join('&',
							enumerable.Cast<object>()
							.Where(mem => mem != null)
							.Select((mem, i) =>
							{
								var (isBaseObject, formattedValue) = FormatValue(mem);
								if (isBaseObject)
								{
									return prefix + name + "=" + HttpUtility.UrlEncode(formattedValue);
								}

								return mem.NestedComplexTypeToQueryString($"{name}{HttpUtility.UrlEncode("[")}{i}{HttpUtility.UrlEncode("]")}.");
							}));
					}

					return prefix + name + '=' + HttpUtility.UrlEncode(ToString(prop.GetValue(obj)!));
				}));
		}
		private static string ToCamelCase(string s) => char.ToLowerInvariant(s[0]) + s[1..];

		private static string ToString(object obj)
		{
			return obj switch
			{
				DateTimeOffset date => date.ToString("o"),
				DateTime date => date.ToString("o", CultureInfo.InvariantCulture),
				DateOnly date => date.ToString("o", CultureInfo.InvariantCulture),
				TimeOnly time => time.ToString("o", CultureInfo.InvariantCulture),
				TimeSpan time => time.ToString("o", CultureInfo.InvariantCulture),
				_ => obj?.ToString() ?? "",
			};
		}

		private static (bool isBaseObject, string formattedValue) FormatValue(object obj)
		{
			if (obj.GetType().IsPrimitive
				|| obj.GetType().IsEnum
				|| obj is string
				|| obj is DateTimeOffset
				|| obj is DateTime
				|| obj is DateOnly
				|| obj is TimeOnly
				|| obj is TimeSpan)
			{
				return (true, ToString(obj));
			}

			return (false, "");
		}
	}
}
