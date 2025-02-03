using System.Collections.Generic;
using BoardLogic;

public static class SelectionResultConverter
{
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