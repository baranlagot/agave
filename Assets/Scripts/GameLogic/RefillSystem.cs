using System;
using System.Collections.Generic;
using System.Linq;
using BoardLogic;

/// <summary>
/// Refills the board with new items.
/// </summary>
public static class RefillSystem
{
    /// <summary>
    /// Applies the refill to the board.
    /// </summary>
    /// <param name="board">The board to refill.</param>
    /// <returns>The commands to apply the refill.</returns>
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

    /// <summary>
    /// Gets a random board item color.
    /// </summary>
    /// <returns>A random board item color.</returns>
    private static BoardItemColor GetRandomBoardItemColor()
    {
        var colors = Enum.GetValues(typeof(BoardItemColor)).Cast<BoardItemColor>().ToList();
        colors.Remove(BoardItemColor.None);
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }

}