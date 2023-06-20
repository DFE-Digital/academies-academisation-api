using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy comments updated correctly.
	/// </summary>
	public class CyCommentsUpdatedCorrectly : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new[]
		{
			(@"update academisation.Project set 
			LocalAuthorityInformationTemplateReturnedDate = '2023-01-01', 
			LocalAuthorityInformationTemplateSentDate = '2023-01-01'                             
			where Id = {0}", new object[] { Id! })
		};

		/// <summary>
		///     Gets a value indicating whether has valid arguments.
		/// </summary>
		public sealed override bool HasValidArguments => !string.IsNullOrWhiteSpace(Id);

		/// <summary>
		///     Gets or sets the id.
		/// </summary>
		public string? Id { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CyCommentsUpdatedCorrectly"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public CyCommentsUpdatedCorrectly(string id)
		{
			Id = id;
			if (!HasValidArguments) throw new ArgumentException();
		}
	}
}
