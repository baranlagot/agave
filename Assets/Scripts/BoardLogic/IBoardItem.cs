namespace BoardLogic
{
    public interface IBoardItem
    {
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