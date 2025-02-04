using System.Collections.Generic;
using BoardLogic;

/// <summary>
/// Converts a stack of board cells to a list of board commands.
/// </summary>
public static class SelectionResultConverter
{
    /// <summary>
    /// Converts a stack of board cells to a list of board commands.
    /// </summary>
    /// <param name="selectedItems">The stack of board cells.</param>
    public static List<IBoardCommand> ConvertToSelectionResult(Stack<BoardCell> selectedItems)
    {
        var commands = new List<IBoardCommand>();
        while (selectedItems.Count > 0)
        {
            var boardCell = selectedItems.Pop();
            commands.Add(new DestroyCommand(boardCell.BoardPosition));
        }

        return commands;
    }
}