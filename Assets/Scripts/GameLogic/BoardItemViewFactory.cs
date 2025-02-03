using UnityEngine;
using BoardLogic;

public class BoardItemViewFactory : IBoardItemViewFactory
{
    private IGameObjectFactory<BoardItemView> gameObjectFactory;

    public BoardItemViewFactory(BoardItemView prefab, int initialSize, Transform parent = null)
    {
        gameObjectFactory = new PoolFactory<BoardItemView>(prefab, initialSize, parent);
    }

    public IBoardItemView CreateBoardItem(IBoardItem boardItem, Vector3 position)
    {
        BoardItemView boardItemView = gameObjectFactory.CreateObject(position, Quaternion.identity);
        boardItemView.Initialize(boardItem, gameObjectFactory);
        return boardItemView;
    }

    public void ReleaseBoardItem(IBoardItemView boardItem)
    {
        gameObjectFactory.ReleaseObject((BoardItemView)boardItem);
    }
}
