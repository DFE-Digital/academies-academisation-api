﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Academies
{
	public record AcademiesTrustsResponse
	{
		public List<Trust> Data { get; set; }
	}
}
