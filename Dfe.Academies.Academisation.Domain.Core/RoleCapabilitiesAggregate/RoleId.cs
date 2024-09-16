﻿using System.ComponentModel;

namespace Dfe.Academies.Academisation.Domain.Core.RoleCapabilitiesAggregate
{
	public enum RoleId
	{
		[Description("Support")]
		Support = 1,
		[Description("ConversionCreation")]
		ConversionCreation = 2,
		[Description("TransferCreation")]
		TransferCreation = 3
	}
}
