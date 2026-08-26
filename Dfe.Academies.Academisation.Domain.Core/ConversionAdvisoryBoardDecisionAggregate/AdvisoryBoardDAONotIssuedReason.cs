namespace Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;

public enum AdvisoryBoardDAONotIssuedReason
{
	SchoolWouldNotBeViableAsAnAcademy = 0,
	ThereAreNoSuitableTrustOptions = 1,
	SchoolAlreadyConvertingAndSufficientlyAdvanced = 2,
	Other = 3
}
