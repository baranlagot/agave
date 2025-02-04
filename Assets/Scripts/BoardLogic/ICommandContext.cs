namespace BoardLogic
{
    /// <summary>
    /// Represents the context of a command.
    /// </summary>
    public interface ICommandContext
    {
        /// <summary>
        /// Destroys the board item at the specified position.
        /// </summary>
        void DestoryBoardItem((int x, int y) position);

        /// <summary>
        /// Moves the board item from the specified position to the target position.
        /// </summary>
        void MoveBoardItem((int x, int y) from, (int x, int y) to);

        /// <summary>
        /// Creates a board item at the specified position.
        /// </summary>
        void CreateBoardItem((int x, int y) position, IBoardItem boardItem);

        /// <summary>
        /// Refills the board at the specified position.
        /// </summary>
        void RefillBoard((int x, int y) finalPos, IBoardItem boardItem, float spawnY);

        /// <summary>
        /// Checks if there are any links remaining on the board.
        /// </summary>
        void CheckLinksRemaining();
    }
}

