﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record ApplicationTrustServiceModel(int id, int UkPrn, string trustName, string? TrustApproverName);
}
