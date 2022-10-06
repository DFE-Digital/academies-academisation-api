using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Dfe.Academies.Academisation.Data.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.ApplicationAggregate.Trusts;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Service.AutoMapper;

public static class AutoMapperSetup
{
	public static void AddMappings(Profile profile) {

		Guard.Against.NullOrEmpty(nameof(profile));

		// Trust mappings
		profile.CreateMap<IJoinTrust, JoinTrustState>();
		profile.CreateMap<JoinTrustState, JoinTrust>();

	}
}
