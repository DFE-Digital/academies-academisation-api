namespace Dfe.Academies.Academisation.IService.ServiceModels.Application;

public record TrustKeyPersonServiceModel(int Id, string Name, DateTime DateOfBirth, string Biography, IEnumerable<TrustKeyPersonRoleServiceModel> Roles);
