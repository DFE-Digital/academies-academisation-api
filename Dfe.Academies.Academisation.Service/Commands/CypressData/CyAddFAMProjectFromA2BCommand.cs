using Dfe.Academies.Academisation.Core;
using MediatR;

namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cy add form a mat project.
	/// </summary>
	public class CyAddFAMProjectFromA2BCommand : CypressDataCommandAbstractBase, IRequest<CommandResult>
	{
		/// <summary>
		///     Gets a value indicating whether has valid arguments.
		/// </summary>
		public override bool HasValidArguments => true;
	}
}
