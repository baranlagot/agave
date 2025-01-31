using System.Collections.Generic;
using BoardLogic;

public struct SelectionResult
{
    public Queue<BoardCell> SelectedItems { get; }

    public SelectionResult(Queue<BoardCell> selectedItems)
    {
        SelectedItems = selectedItems;
    }
}