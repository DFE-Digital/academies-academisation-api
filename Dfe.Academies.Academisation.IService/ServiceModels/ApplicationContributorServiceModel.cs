﻿using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels;

public record ApplicationContributorServiceModel(
	int ContributorId,
	string FirstName,
	string LastName,
	string EmailAddress,
	ContributorRole Role,
	string? OtherRoleName);
