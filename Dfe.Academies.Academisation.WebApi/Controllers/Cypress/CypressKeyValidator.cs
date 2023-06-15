using Dfe.Academies.Academisation.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Dfe.Academies.Academisation.WebApi.Controllers.Cypress
{
	public class CypressKeyValidator : ICypressKeyValidator
	{
		private readonly IHostEnvironment _environment;
		private Guid _cypressKey;

		public CypressKeyValidator(IConfiguration config, IHostEnvironment environment)
		{
			_environment = environment;

			var configValue = config.GetValue<string>("CypressEndpointsKey");
			if (configValue != null)
			{
				if (Guid.TryParse(configValue, out _cypressKey) == false)
				{
					_cypressKey = Guid.Empty;
				}
			}
		}

		/// <summary>
		/// Returns true if all conditions of using the cypress key to gain access to this controllers functionality, passes
		/// </summary>
		/// <param name="cypressKey">The cypress key.</param>
		/// <param name="userKey">The user key.</param>
		/// <returns>A bool.</returns>
		public bool IsKeyValid(string userKey)
		{
			bool userKeyValid = Guid.TryParse(userKey, out Guid userKeyGuid) && userKeyGuid != Guid.Empty;

			return _cypressKey != Guid.Empty &&
				   userKeyValid &&
				   (_environment.IsDevelopment() || _environment.IsStaging()) &&
				   _cypressKey == userKeyGuid;
		}
	}
}
