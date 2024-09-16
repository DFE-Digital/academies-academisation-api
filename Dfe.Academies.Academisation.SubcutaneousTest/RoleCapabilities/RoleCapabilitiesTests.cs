using System.Net;
using Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate;
using Dfe.Academies.Academisation.SubcutaneousTest.Utils;
using Dfe.Academies.Academisation.IService.ServiceModels.RoleCapabilities;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.Academies.Academisation.SubcutaneousTest.RoleCapabilities
{
	public class RoleCapabilitiesTests : ApiIntegrationTestBase
	{
		private readonly HttpClient _client;
		private readonly IRoleInfo _roleInfo;

		public RoleCapabilitiesTests()
		{
			_client = CreateClient();
			_roleInfo = ServiceProvider.GetRequiredService<IRoleInfo>();
		}

		[Theory]
		[InlineData("ConversionCreation")]
		[InlineData("ConversionCreation", "TransferCreation")]
		[InlineData("ConversionCreation", "TransferCreation", "Support")]
		public async Task GetUserRoleCapabilities_ShouldReturnCapabilities(params string[] roles)
		{
			// Arrange   
			var capabilities = new List<RoleCapability>();
			roles.ToList().ForEach(role => capabilities.AddRange(_roleInfo.GetRoleCapabilities(role)));

			// Action
			var httpResponseMessage = await _client.PostAsJsonAsync("role-capabilities/capabilities", roles, CancellationToken);

			// Assert
			await VerifyRoleCapabilities(httpResponseMessage, capabilities.Distinct().ToList());
		}

		private static async Task VerifyRoleCapabilities(HttpResponseMessage httpResponseMessage, List<RoleCapability> capabilities)
		{
			Assert.True(httpResponseMessage.IsSuccessStatusCode);
			Assert.Equal(httpResponseMessage.StatusCode.GetHashCode(), HttpStatusCode.OK.GetHashCode());
			var roleCapabilitiesModel = await httpResponseMessage.ConvertResponseToEnumTypeAsync<RoleCapabilitiesModel>();
			Assert.Equal(roleCapabilitiesModel.Capabilities.Count, capabilities.Count);
			for (int i = 0; i < capabilities.Count; i++)
			{
				Assert.Equal(capabilities[i], roleCapabilitiesModel.Capabilities[i]);
			}
		}
	}
}
