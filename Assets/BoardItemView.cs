using PuzzleLogic;
using UnityEngine;

public class BoardItemView : MonoBehaviour, IBoardItemView
{
    public IBoardItem BoardItem { get; private set; }
    public IGameObjectFactory<BoardItemView> Factory { get; private set; }

    public void Initialize(IBoardItem boardItem, IGameObjectFactory<BoardItemView> factory)
    {
        BoardItem = boardItem;
        Factory = factory;
    }
}
