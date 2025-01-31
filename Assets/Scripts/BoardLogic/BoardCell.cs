
namespace BoardLogic
{
    public struct BoardCell
    {
        public (int x, int y) BoardPosition { get; }
        public bool IsEmpty => boardItem == null;

        public IBoardItem boardItem;

        public BoardCell(IBoardItem boardItem, (int x, int y) boardPosition)
        {
            this.boardItem = boardItem;
            BoardPosition = boardPosition;
        }

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