using BoardLogic;

public struct OnBoardChangedEvent : IEvent
{
    //we dont want external changes on the board
    public readonly Board board;

    public OnBoardChangedEvent(Board board)
    {
        this.board = board;
    }

}
