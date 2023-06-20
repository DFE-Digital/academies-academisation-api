using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy advisory board urls.
	/// </summary>
	public class CyAdvisoryBoardUrlsCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets the sql statements.
		/// </summary>
		public override IEnumerable<(string, object[])> SqlStatements => new[]
		{
			(@"update academisation.Project 
			set LocalAuthorityInformationTemplateReturnedDate = '2023-01-01',
			LocalAuthorityInformationTemplateSentDate = '2023-01-01',
			HeadTeacherBoardDate = '2024-01-01'
			where Id = {0}", new object[] { Id! })
		};


		/// <summary>
		///     Gets a value indicating whether command has valid arguments.
		/// </summary>
		public sealed override bool HasValidArguments => !string.IsNullOrWhiteSpace(Id);

		/// <summary>
		///     Gets or sets the id.
		/// </summary>
		public string? Id { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CyAdvisoryBoardUrlsCommand"/> class.
		/// </summary>
		/// <param name="id">The id.</param>
		public CyAdvisoryBoardUrlsCommand(string id)
		{
			Id = id;
			if (!HasValidArguments) throw new ArgumentException();
		}
	}
}
