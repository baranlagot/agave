namespace BoardLogic
{
    public interface ICommandContext
    {
        void DestoryBoardItem((int x, int y) position);
        void MoveBoardItem((int x, int y) from, (int x, int y) to);
        void CreateBoardItem((int x, int y) position, IBoardItem boardItem);
        void RefillBoard((int x, int y) finalPos, IBoardItem boardItem, float spawnY);
        void CheckLinksRemaining();
    }
}
