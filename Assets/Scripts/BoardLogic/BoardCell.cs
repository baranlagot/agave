
namespace BoardLogic
{
    /// <summary>
    /// Represents a cell on the game board.
    /// </summary>
    public struct BoardCell
    {
        /// <summary>
        /// Gets the position of the cell on the board.
        /// </summary>
        public (int x, int y) BoardPosition { get; }

        /// <summary>
        /// Gets a value indicating whether the cell is empty.
        /// </summary>
        public bool IsEmpty => boardItem == null;

        /// <summary>   
        /// Gets the board item in the cell.
        /// </summary>
        public IBoardItem boardItem;

        /// <summary>
        /// Initializes a new instance of the BoardCell class.
        /// </summary>
        /// <param name="boardItem">The board item in the cell.</param>
        public BoardCell(IBoardItem boardItem, (int x, int y) boardPosition)
        {
            this.boardItem = boardItem;
            BoardPosition = boardPosition;
        }

        /// <summary>   
        /// Determines if two BoardCell objects are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current BoardCell.</param>
        /// <returns>True if the objects are equal, false otherwise.</returns>
        public override readonly bool Equals(object obj)
        {
            if (obj is BoardCell cell)
            {
                return cell.BoardPosition == BoardPosition;
            }

            return false;
        }

    }
}