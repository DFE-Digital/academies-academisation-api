﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecision : IConversionAdvisoryBoardDecision
{
	private ConversionAdvisoryBoardDecision(AdvisoryBoardDecisionDetails details)
	{
		AdvisoryBoardDecisionDetails = details;
	}
	
	public ConversionAdvisoryBoardDecision(int id, AdvisoryBoardDecisionDetails details) : this(details)
	{
		Id = id;
	}

	private static readonly CreateConversionAdvisoryBoardDecisionValidator CreateConversionAdvisoryBoardDecisionValidator  = new();
	
	public AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; }
	public int Id { get; private set;  }
	
	internal static CreateResult<IConversionAdvisoryBoardDecision> Create(AdvisoryBoardDecisionDetails details)
	{
		var validationResult = CreateConversionAdvisoryBoardDecisionValidator.Validate(details);

		return validationResult.IsValid
			? new CreateSuccessResult<IConversionAdvisoryBoardDecision>(
				new ConversionAdvisoryBoardDecision(details))
			: new CreateValidationErrorResult<IConversionAdvisoryBoardDecision>(
				validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));
	}

	public void SetId(int id) => Id = Id == default 
		? id 
		: throw new InvalidOperationException("Cannot assign an id when the id has already been set");
}
