using BoardLogic;
using UnityEngine;

public class DestroyCommand : IBoardCommand
{
    private (int x, int y) pos;
    public DestroyCommand((int, int) pos) => this.pos = pos;
    public string LogMessage => $"Destroy cube at {pos}";

    public void LogCommand()
    {
        Debug.Log(LogMessage);
    }

    public void Execute(ICommandContext context)
    {
        context.DestoryBoardItem(pos);
    }
}