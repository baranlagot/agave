
namespace BoardLogic
{
    public class DefaultBoardItem : IBoardItem
    {
        public BoardItemColor Color { get; private set; }
        public string Name => Color.ToString();

        public DefaultBoardItem(BoardItemColor color)

        {
            Color = color;
        }

        public IBoardItem Clone()
        {
            return new DefaultBoardItem(Color);
        }

    }
}