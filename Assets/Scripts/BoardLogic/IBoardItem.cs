namespace BoardLogic
{
    /// <summary>
    /// Represents a board item.
    /// </summary>
    public interface IBoardItem
    {
        /// <summary>
        /// Gets the name of the board item.
        public string Name { get; }

        BoardItemColor Color { get; }
        public IBoardItem Clone();
    }

    public enum BoardItemColor
    {
        Red,
        Green,
        Blue,
        Yellow,
        None
    }
}