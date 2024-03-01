using Dfe.Academies.Academisation.Core;
using Dfe.Academies.Academisation.Domain.Core.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.Domain.Core.ProjectAggregate;
using Dfe.Academies.Academisation.Domain.SeedWork;
using Dfe.Academies.Academisation.IDomain.ConversionAdvisoryBoardDecisionAggregate;
using Dfe.Academies.Academisation.IDomain.ProjectAggregate;

namespace Dfe.Academies.Academisation.Domain.ConversionAdvisoryBoardDecisionAggregate;

public class ConversionAdvisoryBoardDecision : IConversionAdvisoryBoardDecision, IAggregateRoot
{
	protected ConversionAdvisoryBoardDecision(){}
	private ConversionAdvisoryBoardDecision(AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons
		)
	{
		AdvisoryBoardDecisionDetails = details;
		_deferredReasons = deferredReasons.ToList();
		_declinedReasons = declinedReasons.ToList();
		_withdrawnReasons = withdrawnReasons.ToList();
	}

	public ConversionAdvisoryBoardDecision(
		int id,
		AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons,
		DateTime createdOn,
		DateTime lastModifiedOn) : this(details, deferredReasons, declinedReasons, withdrawnReasons)
	{
		Id = id;
		CreatedOn = createdOn;
		LastModifiedOn = lastModifiedOn;
	}

	private static readonly ConversionAdvisoryBoardDecisionValidator Validator = new();

	public AdvisoryBoardDecisionDetails AdvisoryBoardDecisionDetails { get; private set; }
	public int Id { get; private set; }
	public DateTime CreatedOn { get; }
	public DateTime LastModifiedOn { get; }

	public IEnumerable<AdvisoryBoardDeclinedReasonDetails> DeclinedReasons => _declinedReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardDeclinedReasonDetails> IConversionAdvisoryBoardDecision.DeclinedReasons => _declinedReasons.AsReadOnly();
	private readonly List<AdvisoryBoardDeclinedReasonDetails> _declinedReasons;

	public IEnumerable<AdvisoryBoardDeferredReasonDetails> DeferredReasons => _deferredReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardDeferredReasonDetails> IConversionAdvisoryBoardDecision.DeferredReasons => _deferredReasons.AsReadOnly();
	private readonly List<AdvisoryBoardDeferredReasonDetails> _deferredReasons;

	public IEnumerable<AdvisoryBoardWithdrawnReasonDetails> WithdrawnReasons => _withdrawnReasons.AsReadOnly();
	IReadOnlyCollection<AdvisoryBoardWithdrawnReasonDetails> IConversionAdvisoryBoardDecision.WithdrawnReasons => _withdrawnReasons.AsReadOnly();
	private readonly List<AdvisoryBoardWithdrawnReasonDetails> _withdrawnReasons;

	internal static CreateResult Create(AdvisoryBoardDecisionDetails details,
				IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons)
	{
		var decision = new ConversionAdvisoryBoardDecision(details, deferredReasons, declinedReasons, withdrawnReasons);

		var validationResult = Validator.Validate(decision);

		return validationResult.IsValid
			? new CreateSuccessResult<IConversionAdvisoryBoardDecision>(
				decision)
			: new CreateValidationErrorResult(
				validationResult.Errors.Select(r => new ValidationError(r.PropertyName, r.ErrorMessage)));
	}

	public CommandResult Update(AdvisoryBoardDecisionDetails details,
		IEnumerable<AdvisoryBoardDeferredReasonDetails> deferredReasons,
		IEnumerable<AdvisoryBoardDeclinedReasonDetails> declinedReasons,
		IEnumerable<AdvisoryBoardWithdrawnReasonDetails> withdrawnReasons)
	{
		AdvisoryBoardDecisionDetails = details;
		_declinedReasons.Clear(); 
		_declinedReasons.AddRange(declinedReasons.ToList());
		_deferredReasons.Clear();
		_deferredReasons.AddRange(deferredReasons.ToList());
		_declinedReasons.Clear();
		_withdrawnReasons.AddRange(withdrawnReasons.ToList());

		var validationResult = Validator.Validate(this);

		if (!validationResult.IsValid)
		{
			var validationError = validationResult.Errors
				.Select(x => new ValidationError(x.PropertyName, x.ErrorMessage));

			return new CommandValidationErrorResult(validationError);
		}

		
		return new CommandSuccessResult();
	}

	public void SetId(int id)
	{
		Id = Id == default
		? id
		: throw new InvalidOperationException("Cannot assign an id when the id has already been set");
	}
}
