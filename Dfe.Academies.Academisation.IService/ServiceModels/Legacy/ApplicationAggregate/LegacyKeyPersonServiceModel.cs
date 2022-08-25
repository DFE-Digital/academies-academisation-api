namespace Dfe.Academies.Academisation.IService.ServiceModels.Legacy.ApplicationAggregate;

public record LegacyKeyPersonServiceModel(
	string? Name = null,
	DateTime? KeyPersonDateOfBirth = null,
	string? KeyPersonBiography = null,
	bool? KeyPersonCeoExecutive = null,
	bool? KeyPersonChairOfTrust = null,
	bool? KeyPersonFinancialDirector = null,
	bool? KeyPersonMember = null,
	bool? KeyPersonOther = null,
	bool? KeyPersonTrustee = null
);
