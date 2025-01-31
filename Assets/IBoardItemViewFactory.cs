using PuzzleLogic;
using UnityEngine;

public interface IBoardItemViewFactory
{
    IBoardItemView CreateBoardItem(IBoardItem boardItem, Vector3 position);
    void ReleaseBoardItem(IBoardItemView boardItem);
}
