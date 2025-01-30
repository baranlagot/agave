using PuzzleLogic;

namespace BoardLogic
{
    public struct BoardCell
    {
        public IBoardItem boardItem;
        public bool IsEmpty => boardItem == null;

        public BoardCell(IBoardItem boardItem)
        {
            this.boardItem = boardItem;
        }
    }
}