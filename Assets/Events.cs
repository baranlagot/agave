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

public struct OnMoveChangedEvent : IEvent
{
    public int movesLeft;
    public int scoreTotal;

    public OnMoveChangedEvent(int movesLeft, int scoreTotal)
    {
        this.movesLeft = movesLeft;
        this.scoreTotal = scoreTotal;
    }
}

public struct OnGameWinEvent : IEvent
{
    public string gameWinMessage
    ;

    public OnGameWinEvent(string gameWinMessage)
    {
        this.gameWinMessage = gameWinMessage;
    }
}

public struct OnGameLoseEvent : IEvent
{
    public string gameLoseMessage;

    public OnGameLoseEvent(string gameLoseMessage)
    {
        this.gameLoseMessage = gameLoseMessage;
    }
}

public struct OnNextLevelClickedEvent : IEvent
{
}

public struct OnRetryLevelClickedevent : IEvent
{
}