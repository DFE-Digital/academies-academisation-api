﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application
{
	public record SetJoinTrustDetailsCommand(string trustName, int UkPrn, bool? changesToTrust, string? changesToTrustExplained);
}
