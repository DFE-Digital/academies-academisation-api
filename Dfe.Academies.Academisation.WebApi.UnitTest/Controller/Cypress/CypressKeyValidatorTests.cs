using System;
using Dfe.Academies.Academisation.WebApi.Middleware;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Xunit;

namespace Dfe.Academies.Academisation.WebApi.UnitTest.Controller.Cypress
{
	/// <summary>
	///     The cypress key validator tests.
	/// </summary>
	public class CypressKeyValidatorTests
	{
		/// <summary>
		///     Are the key valid_ when_ empty cypress key_ and_ other_ args_ valid_ returns_ false.
		/// </summary>
		/// <param name="environmentName">The environment name.</param>
		/// <param name="cypressKey">The cypress key.</param>
		[Theory]
		[InlineData("Development", default)]
		[InlineData("Development", "")]
		[InlineData("Development", "not-a-guid")]
		[InlineData("Staging", default)]
		[InlineData("Staging", "")]
		[InlineData("Staging", "not-a-guid")]
		public void IsKeyValid_When_EmptyCypressKey_And_Other_Args_Valid_Returns_False(string environmentName, string cypressKey)
		{
			IHostEnvironment? env = Mock.Of<IHostEnvironment>(x => x.EnvironmentName == environmentName);

			IConfigurationSection? fakeConfigSection = Mock.Of<IConfigurationSection>(x => x.Key == "CypressEndpointsKey" && x.Value == cypressKey);
			IConfiguration? config = Mock.Of<IConfiguration>(x => x.GetSection("CypressEndpointsKey") == fakeConfigSection);

			var sut = new CypressKeyValidator(config, env);
			bool result = sut.IsKeyValid(Guid.NewGuid().ToString());

			result.Should().BeFalse();
		}

		/// <summary>
		///     Are the key valid_ when_ keys_ valid_ but_ production_ returns_ false.
		/// </summary>
		[Fact]
		public void IsKeyValid_When_Keys_Valid_But_Production_Returns_False()
		{
			string cypressKeyString = "de59ad71-1a50-4c07-a50f-ef14519977d2";
			IHostEnvironment? env = Mock.Of<IHostEnvironment>(x => x.EnvironmentName == "Production");

			IConfigurationSection? fakeConfigSection = Mock.Of<IConfigurationSection>(x => x.Key == "CypressEndpointsKey" && x.Value == cypressKeyString);
			IConfiguration? config = Mock.Of<IConfiguration>(x => x.GetSection("CypressEndpointsKey") == fakeConfigSection);

			var sut = new CypressKeyValidator(config, env);
			bool result = sut.IsKeyValid(Guid.Parse(cypressKeyString).ToString());

			result.Should().BeFalse();
		}

		/// <summary>
		///     Are the key valid_ when_ keys_ valid_ and_ not_ production_ returns_ true.
		/// </summary>
		/// <param name="environmentName">The environment name.</param>
		[Theory]
		[InlineData("Development")]
		[InlineData("Staging")]
		public void IsKeyValid_When_Keys_Valid_And_Not_Production_Returns_True(string environmentName)
		{
			string cypressKeyString = "de59ad71-1a50-4c07-a50f-ef14519977d2";
			IHostEnvironment? env = Mock.Of<IHostEnvironment>(x => x.EnvironmentName == environmentName);

			IConfigurationSection? fakeConfigSection = Mock.Of<IConfigurationSection>(x => x.Key == "CypressEndpointsKey" && x.Value == cypressKeyString);
			IConfiguration? config = Mock.Of<IConfiguration>(x => x.GetSection("CypressEndpointsKey") == fakeConfigSection);

			var sut = new CypressKeyValidator(config, env);
			bool result = sut.IsKeyValid(Guid.Parse(cypressKeyString).ToString());

			result.Should().BeTrue();
		}

		[Theory]
		[InlineData("Development", "de59ad71-1a50-4c07-a50f-ef14519977d2")]
		[InlineData("Staging", "de59ad71-1a50-4c07-a50f-ef14519977d2")]
		public void IsKeyValid_When_UserKeyDoesNotMatch_Returns_False(string environmentName, string userKey)
		{
			IHostEnvironment? env = Mock.Of<IHostEnvironment>(x => x.EnvironmentName == environmentName);

			IConfigurationSection? fakeConfigSection = Mock.Of<IConfigurationSection>(x => x.Key == "CypressEndpointsKey" && x.Value == Guid.NewGuid().ToString());
			IConfiguration? config = Mock.Of<IConfiguration>(x => x.GetSection("CypressEndpointsKey") == fakeConfigSection);

			var sut = new CypressKeyValidator(config, env);
			bool result = sut.IsKeyValid(userKey);

			result.Should().BeFalse();
		}
	}
}
