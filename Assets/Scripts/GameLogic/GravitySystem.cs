using System.Collections.Generic;
using BoardLogic;

public static class GravitySystem
{
    public static List<IBoardCommand> ApplyGravity(Board board)
    {
        var commands = new List<IBoardCommand>();

        for (int x = 0; x < board.Width; x++)
        {
            int writeY = 0;
            for (int y = 0; y < board.Height; y++)
            {
                var cell = board.GetCell(x, y);
                if (!cell.IsEmpty)
                {
                    if (y != writeY)
                    {
                        board.SetCell(x, writeY, cell);
                        board.SetCell(x, y, new BoardCell(null, (x, y)));
                        commands.Add(new MoveCommand((x, y), (x, writeY)));
                    }
                    writeY++;
                }
            }
        }

        return commands;
    }
}
