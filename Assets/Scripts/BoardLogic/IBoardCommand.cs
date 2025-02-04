namespace BoardLogic
{
    /// <summary>
    /// Represents a command that can be executed on the board.
    /// </summary>
    public interface IBoardCommand
    {
        /// <summary>
        /// Gets the log message for the command.
        /// </summary>
        public string LogMessage { get; }

        /// <summary>
        /// Executes the command on the board.
        /// </summary>
        /// <param name="context">The context of the command.</param>
        public void Execute(ICommandContext context);
    }

}