using System.Collections.Generic;
using BoardLogic;

/// <summary>
/// Represents the result of a selection.
/// </summary>
public struct SelectionResult
{
    /// <summary>
    /// Gets the selected items.
    /// </summary>
    public Queue<BoardCell> SelectedItems { get; }

    /// <summary>
    /// Initializes a new instance of the SelectionResult struct.
    /// </summary>
    /// <param name="selectedItems">The selected items.</param>
    public SelectionResult(Queue<BoardCell> selectedItems)
    {
        SelectedItems = selectedItems;
    }
}