using BoardLogic;

/// <summary>
/// Represents a view for a board item.
/// </summary>
public interface IBoardItemView
{
    /// <summary>
    /// Gets the board item.
    /// </summary>
    IBoardItem BoardItem { get; }

    /// <summary>
    /// Gets the factory.
    /// </summary>
    IGameObjectFactory<BoardItemView> Factory { get; }
}