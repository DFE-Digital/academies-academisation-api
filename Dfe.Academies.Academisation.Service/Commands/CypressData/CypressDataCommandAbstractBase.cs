namespace Dfe.Academies.Academisation.Service.Commands.CypressData
{
	/// <summary>
	///     The cypress data base command.
	/// </summary>
	public abstract class CypressDataCommandAbstractBase
	{
		/// <summary>
		///     Gets the sql to execute for the command, and the accompanying parameters if any
		/// </summary>
		public abstract IEnumerable<(string statement, object[] parameters)> SqlStatements { get; }

		/// <summary>
		///     Gets a value indicating whether command has valid arguments. If it does not the command will not be executed
		/// </summary>
		public abstract bool HasValidArguments { get; }
	}
}
