using Dfe.Academies.Academisation.Domain.Core.ApplicationAggregate;

namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record TrustKeyPersonServiceModel(int Id, string FirstName, string Surname, DateTime? DateOfBirth, string? ContactEmailAddress, KeyPersonRole Role, string TimeInRole, string Biography);
