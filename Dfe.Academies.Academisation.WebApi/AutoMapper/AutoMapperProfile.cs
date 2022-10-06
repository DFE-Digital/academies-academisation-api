using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dfe.Academies.Academisation.Service.AutoMapper;

namespace Dfe.Academies.Academisation.WebApi.AutoMapper
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile() => AutoMapperSetup.AddMappings(this);
	}
}
