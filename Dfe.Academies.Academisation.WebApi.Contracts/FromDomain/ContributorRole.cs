﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Academies.Academisation.WebApi.Contracts.FromDomain;

public enum ContributorRole
{
	// ToDo: discuss with David et al whether we need to future proof this with more specified roles
	ChairOfGovernors = 1,
	Other = 2
}
