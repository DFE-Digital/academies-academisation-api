﻿using Dfe.Academies.Academisation.Data;
using Dfe.Academies.Academisation.Service.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dfe.Academies.Academisation.Service.Behaviours;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
	private readonly AcademisationContext _dbContext;

	public TransactionBehavior(AcademisationContext dbContext,
		ILogger<TransactionBehavior<TRequest, TResponse>> logger)
	{
		_dbContext = dbContext ?? throw new ArgumentException(nameof(AcademisationContext));
		_logger = logger ?? throw new ArgumentException(nameof(ILogger));
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var response = default(TResponse);
		var typeName = request.GetGenericTypeName();

		try
		{
			if (_dbContext.HasActiveTransaction)
			{
				return await next();
			}

			var strategy = _dbContext.Database.CreateExecutionStrategy();

			await strategy.ExecuteAsync(async () =>
			{
				Guid transactionId;

				await using var transaction = await _dbContext.BeginTransactionAsync();
				using (_logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction.TransactionId) }))
				{
					_logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

					response = await next();

					_logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

					await _dbContext.CommitTransactionAsync(transaction);

					transactionId = transaction.TransactionId;
				}
				// if we were publishing events to a bus it would go here
				//await _orderingIntegrationEventService.PublishEventsThroughEventBusAsync(transactionId);
			});

			return response;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

			throw;
		}
	}
}

