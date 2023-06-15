namespace Dfe.Academies.Academisation.WebApi.Controllers.Cypress
{
	public interface ICypressKeyValidator
	{
		/// <summary>
		/// Returns true if all conditions of using the cypress key to gain access to this controllers functionality, passes
		/// </summary>
		/// <param name="cypressKey">The cypress key.</param>
		/// <param name="userKey">The user key.</param>
		/// <returns>A bool.</returns>
		bool IsKeyValid(string userKey);
	}
}
