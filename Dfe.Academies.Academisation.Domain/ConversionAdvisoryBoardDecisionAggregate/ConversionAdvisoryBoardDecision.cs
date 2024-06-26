﻿using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecision : Entity, IConversionAdvisoryBoardDecision, IAggregateRoot
{
	protected ConversionAdvisoryBoardDecision() { }
	private ConversionAdvisoryBoardDecision(AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails>? deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails>? declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails>? withdrawnReasons,
		IEnumerable<AdvisoryBoardDAORevokedReasonDetails>? daoRevokedReasons
		)
	{
		AdvisoryBoardDecisionDetails = details;
		_deferredReasons = deferredReasons?.Any() ?? false ? deferredReasons.ToList() : _deferredReasons;
		_declinedReasons = declinedReasons?.Any() ?? false ? declinedReasons.ToList() : _declinedReasons;
		_withdrawnReasons = withdrawnReasons?.Any() ?? false ? withdrawnReasons.ToList() : _withdrawnReasons;
		_daoRevokedReasons = daoRevokedReasons?.Any() ?? false ? daoRevokedReasons.ToList() : _daoRevokedReasons;
	}

	public ConversionAdvisoryBoardDecision(
		int id,
		AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons,
		IEnumerable<AdvisoryBoardDAORevokedReasonDetails>? daoRevokedReasons,
		DateTime createdOn,
		DateTime lastModifiedOn) : this(details, deferredReasons, declinedReasons, withdrawnReasons, daoRevokedReasons)
	{
		Id = id;
		CreatedOn = createdOn;
		LastModifiedOn = lastModifiedOn;
	}

	private static readonly ConversionAdvisoryBoardDecisionValidator Validator = new();

	public AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; private set; }

	public IEnumerable<AdvisoryBoardDeclinedReasonDetails> DeclinedReasons => _declinedReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardDeclinedReasonDetails> IConversionAdvisoryBoardDecision.DeclinedReasons => _declinedReasons.AsReadOnly();
	private readonly List<AdvisoryBoardDeclinedReasonDetails> _declinedReasons = new();

	public IEnumerable<AdvisoryBoardDeferredReasonDetails> DeferredReasons => _deferredReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardDeferredReasonDetails> IConversionAdvisoryBoardDecision.DeferredReasons => _deferredReasons.AsReadOnly();
	private readonly List<AdvisoryBoardDeferredReasonDetails> _deferredReasons = new();

	public IEnumerable<AdvisoryBoardWithdrawnReasonDetails> WithdrawnReasons => _withdrawnReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardWithdrawnReasonDetails> IConversionAdvisoryBoardDecision.WithdrawnReasons => _withdrawnReasons.AsReadOnly();
	private readonly List<AdvisoryBoardWithdrawnReasonDetails> _withdrawnReasons = new();
	public IEnumerable<AdvisoryBoardDAORevokedReasonDetails> DaoRevokedReasons => _daoRevokedReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardDAORevokedReasonDetails> IConversionAdvisoryBoardDecision.DAORevokedReasons => _daoRevokedReasons.AsReadOnly();
	private readonly List<AdvisoryBoardDAORevokedReasonDetails> _daoRevokedReasons = new();


	internal static CreateResult Create(AdvisoryBoardDecisionDetails details,
				IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons,
		IEnumerable<AdvisoryBoardDAORevokedReasonDetails> daoRevokedReasons
		)
	{
		var decision = new ConversionAdvisoryBoardDecision(details, deferredReasons, declinedReasons, withdrawnReasons, daoRevokedReasons);

		var validationResult = Validator.Validate(decision);

		return validationResult.IsValid
			? new CreateSuccessResult<IConversionAdvisoryBoardDecision>(
				decision)
			: new CreateValidationErrorResult(
				validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));
	}

	public CommandResult Update(AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails>? deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails>? declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails>? withdrawnReasons,
		IEnumerable<AdvisoryBoardDAORevokedReasonDetails>? daoRevokedReasons
		)
	{
		// create a new decision to validate intended state
		// if validation fails do not change internal state and raise validation error result
		var decision = new ConversionAdvisoryBoardDecision(details, deferredReasons, declinedReasons, withdrawnReasons, daoRevokedReasons);

		var validationResult = Validator.Validate(decision);

		if (!validationResult.IsValid)
		{
			var validationError = validationResult.Errors
				.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage));

			return new CommandValidationErrorResult(validationError);
		}

		AdvisoryBoardDecisionDetails = details;
		_declinedReasons.Clear();
		_declinedReasons.AddRange(declinedReasons?.Any() ?? false ? declinedReasons.ToList() : _declinedReasons);
		_deferredReasons.Clear();
		_deferredReasons.AddRange(deferredReasons?.Any() ?? false ? deferredReasons.ToList() : _deferredReasons);
		_withdrawnReasons.Clear();
		_withdrawnReasons.AddRange(withdrawnReasons?.Any() ?? false ? withdrawnReasons.ToList() : _withdrawnReasons);
		_daoRevokedReasons.Clear();
		_daoRevokedReasons.AddRange(daoRevokedReasons?.Any() ?? false ? daoRevokedReasons.ToList() : _daoRevokedReasons);

		return new CommandSuccessResult();
	}

	public void SetId(int id)
	{
		Id = Id == default
		? id
		: throw new InvalidOperationException("Cannot assign an id when the id has already been set");
	}
}
