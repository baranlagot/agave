using UnityEngine;
using BoardLogic;

/// <summary>
/// Represents a factory for creating board item views. 
/// </summary>
public interface IBoardItemViewFactory
{
    /// <summary>
    /// Creates a board item view.
    /// </summary>
    /// <param name="boardItem">The board item.</param>
    /// <param name="position">The position of the board item.</param>
    IBoardItemView CreateBoardItem(IBoardItem boardItem, Vector3 position);

    /// <summary>
    /// Releases a board item view.
    /// </summary>
    /// <param name="boardItem">The board item view to release.</param>
    void ReleaseBoardItem(IBoardItemView boardItem);
}
