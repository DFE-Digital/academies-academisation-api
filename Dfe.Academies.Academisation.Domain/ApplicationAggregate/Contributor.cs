﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork.Dynamics;
using Dfe.Academies.Academisation.IDomain.ApplicationAggregate;

namespace Dfe.Academies.Academisation.Domain.ApplicationAggregate;

public class Contributor : DynamicsApplicationEntity, IContributor
{
	protected Contributor() { }
	internal Contributor(ContributorDetails details)
	{
		Details = details;
	}
	internal Contributor(int id, ContributorDetails details)
	{
		Id = id;
		Details = details;
	}

	public ContributorDetails Details { get; private set; }
	public void Update(ContributorDetails details)
	{
		this.Details = details;
	}
}
