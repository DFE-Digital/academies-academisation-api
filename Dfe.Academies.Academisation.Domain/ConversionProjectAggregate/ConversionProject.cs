using Dfe.Academies.Academisation.IDomain.ConversionProjectAggregate;
using ValidationException = FluentValidation.ValidationException;

namespace Dfe.Academies.Academisation.Domain.ConversionProjectAggregate;

public class ConversionProject : IConversionProject
{
	public int Id { get; set; }

	public IAdvisoryBoardDecision? AdvisoryBoardDecision { get; private set; }

	private CreateAdvisoryBoardDecisionValidator CreateAdvisoryBoardDecisionValidator { get; } = new();

	private ConversionProject(int projectId)
	{
		Id = projectId;
	}

	internal static async Task<ConversionProject> Create(int projectId)
	{
		//var validationResult = await CreateValidator.ValidateAsync(details);

		// if (validationResult.IsValid)
		// {
		//     return new(projectId, details);
		// }

		// throw new ValidationException(validationResult.ToString());


		await Task.CompletedTask;
		return new(projectId);
	}

	public async Task AddAdvisoryBoardDecision(IAdvisoryBoardDecisionDetails details)
	{
		var validationResult = await CreateAdvisoryBoardDecisionValidator.ValidateAsync(details);

		if (!validationResult.IsValid) throw new ValidationException(validationResult.ToString());

		AdvisoryBoardDecision = new AdvisoryBoardDecision(Id, details);
	}
}