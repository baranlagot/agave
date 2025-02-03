using System;
using System.Collections.Generic;
using System.Linq;
using BoardLogic;

public static class RefillSystem
{
    public static List<IBoardCommand> ApplyRefill(Board board)
    {
        var commands = new List<IBoardCommand>();

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                var cell = board.GetCell(x, y);
                if (cell.IsEmpty)
                {
                    IBoardItem boardItem = new DefaultBoardItem(GetRandomBoardItemColor());

                    float spawnAtY = board.Height;

                    //place the new item at the board
                    board.SetCell(x, y, new BoardCell(boardItem, (x, y)));

                    commands.Add(new RefillBoardCommand((x, y), boardItem, spawnAtY));
                }
            }
        }
        commands.Add(new CheckLinksCommand());
        return commands;
    }

    private static BoardItemColor GetRandomBoardItemColor()
    {
        var colors = Enum.GetValues(typeof(BoardItemColor)).Cast<BoardItemColor>().ToList();
        colors.Remove(BoardItemColor.None);
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }

}