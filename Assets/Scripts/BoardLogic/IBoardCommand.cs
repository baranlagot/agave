namespace BoardLogic
{
    public interface IBoardCommand
    {
        public string LogMessage { get; }
        public void Execute(ICommandContext context);
    }

}