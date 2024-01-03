﻿using Dfe.Academies.Academisation.IService.ServiceModels.Academies;

namespace Dfe.Academies.Academisation.IService.Query
{
	public interface IAcademiesQueryService
	{
		Task<Establishment?> GetEstablishment(int urn);
		Task<Trust?> GetTrust(string ukprn);
	}
}