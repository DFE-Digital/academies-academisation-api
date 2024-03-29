﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IDomain.Services;

public interface IApplicationSubmissionService
{
	CommandOrCreateResult SubmitApplication(IApplication application);
}
