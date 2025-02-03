using BoardLogic;
using UnityEngine;

public class DestroyCommand : IBoardCommand
{
    private (int x, int y) pos;
    public DestroyCommand((int, int) pos) => this.pos = pos;
    public string LogMessage => $"Destroy board item at {pos}";

    public void LogCommand()
    {
        Debug.Log(LogMessage);
    }

    public void Execute(ICommandContext context)
    {
        context.DestoryBoardItem(pos);
    }
}

public class MoveCommand : IBoardCommand
{
    private (int x, int y) from;
    private (int x, int y) to;
    public string LogMessage => $"Move board item from {from} to {to}";

    public MoveCommand((int x, int y) from, (int x, int y) to)
    {
        this.from = from;
        this.to = to;
    }

    public void LogCommand()
    {
        Debug.Log($"Move board item from {from} to {to}");
    }

    public void Execute(ICommandContext context)
    {
        context.MoveBoardItem(from, to);
    }
}

public class RefillBoardCommand : IBoardCommand
{
    private (int x, int y) finalGridPos;
    private IBoardItem boardItem;
    private float spawnY;
    public string LogMessage => $"Create falling cube at {finalGridPos}";

    public RefillBoardCommand((int x, int y) finalGridPos, IBoardItem boardItem, float spawnY)
    {
        this.finalGridPos = finalGridPos;
        this.boardItem = boardItem;
        this.spawnY = spawnY;
    }


    public void Execute(ICommandContext context)
    {
        context.RefillBoard(finalGridPos, boardItem, spawnY);
    }
}

public class CheckLinksCommand : IBoardCommand
{
    public string LogMessage => $"Checking links";

    public void Execute(ICommandContext context)
    {
        context.CheckLinksRemaining();
    }
}