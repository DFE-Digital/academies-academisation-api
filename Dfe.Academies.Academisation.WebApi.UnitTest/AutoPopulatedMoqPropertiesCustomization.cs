using AutoFixture.Kernel;
using AutoFixture;
using System.Reflection;
using AutoFixture.AutoMoq;

namespace Dfe.Academies.Academisation.WebApi.UnitTest
{
	public class AutoPopulatedMoqPropertiesCustomization : ICustomization
	{
		public void Customize(IFixture fixture)
		{
			fixture.Customizations.Add(
				new PropertiesPostprocessor(
					new MockPostprocessor(
						new AutoFixture.Kernel.MethodInvoker(
							new MockConstructorQuery()))));
			fixture.ResidueCollectors.Add(new MockRelay());
		}
	}

}
