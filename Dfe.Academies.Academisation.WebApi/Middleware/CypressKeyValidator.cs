namespace Dfe.Academies.Academisation.WebApi.Middleware
{
	public class CypressKeyValidator : ICypressKeyValidator
	{
		private readonly IHostEnvironment _environment;
		private readonly Guid _cypressKey;

		public CypressKeyValidator(IConfiguration config, IHostEnvironment environment)
		{
			_environment = environment;

			string? configValue = config.GetValue<string>("CypressEndpointsKey");
			if (configValue == null)
			{
				return;
			}

			if (Guid.TryParse(configValue, out _cypressKey) == false)
			{
				_cypressKey = Guid.Empty;
			}
		}

		/// <summary>
		/// Returns true if all conditions of using the cypress key to gain access to this controllers functionality, passes
		/// </summary>
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
