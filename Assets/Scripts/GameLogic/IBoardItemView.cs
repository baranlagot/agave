using BoardLogic;

public interface IBoardItemView
{
    IBoardItem BoardItem { get; }
    IGameObjectFactory<BoardItemView> Factory { get; }
}