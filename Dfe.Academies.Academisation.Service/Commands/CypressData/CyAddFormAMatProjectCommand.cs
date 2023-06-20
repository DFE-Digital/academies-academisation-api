using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy add form a mat project.
	/// </summary>
	public class CyAddFormAMatProjectCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements =>
			new[]
			{
				(@"UPDATE [academisation].[ConversionApplication]
				SET [academisation].[ConversionApplication].ApplicationStatus = 'InProgress'
				SELECT TOP (1000) [Id]
					  ,[ApplicationType]
					  ,[CreatedOn]
					  ,[LastModifiedOn]
					  ,[ApplicationStatus]
					  ,[FormTrustId]
					  ,[JoinTrustId]
					  ,[ApplicationSubmittedDate]
					  ,[DynamicsApplicationId]
					  ,[ApplicationReference]
				  FROM [academisation].[ConversionApplication]
				  WHERE [academisation].[ConversionApplication].ApplicationReference = 'A2B_31'",
					Array.Empty<object>())
			};

		/// <summary>
		///     Gets a value indicating whether has valid arguments.
		/// </summary>
		public override bool HasValidArguments => true;
	}
}
