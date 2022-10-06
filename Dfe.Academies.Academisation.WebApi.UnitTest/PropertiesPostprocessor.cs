using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Kernel;

namespace Dfe.Academies.Academisation.WebApi.UnitTest
{
	public class PropertiesPostprocessor : ISpecimenBuilder
	{
		private readonly ISpecimenBuilder builder;

		public PropertiesPostprocessor(ISpecimenBuilder builder)
		{
			this.builder = builder;
		}

		public object Create(object request, ISpecimenContext context)
		{
			dynamic s = this.builder.Create(request, context);
			if (s is NoSpecimen)
				return s;

			s.SetupAllProperties();
			return s;
		}
	}
}
