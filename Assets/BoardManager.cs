using System.Collections;
using System.Collections.Generic;
using BoardLogic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private BoardItemView boardItemViewPrefab;
    [SerializeField] private GameObject backgroundTilePrefab;

    private Dictionary<(int, int), BoardItemView> boardItemViews = new Dictionary<(int, int), BoardItemView>();

    private IBoardItemViewFactory boardItemViewFactory;
    private Board board;

    private float boardItemSize = 2.5f;

    private float xOffset => (width * boardItemSize) / 2f - boardItemSize / 2f;
    private float yOffset => (height * boardItemSize) / 2f - boardItemSize / 2f;

    private void Awake()
    {
        board = new Board(width, height);
        board.InitializeRandom();
        boardItemViewFactory = new BoardItemViewFactory(boardItemViewPrefab, width * height, transform);
        CreateVisualsFromBoard();
    }

    private void CreateVisualsFromBoard()
    {
        boardItemViews.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var cell = board.GetCell(x, y);
                if (!cell.IsEmpty)
                {
                    CreateBoardItemViewAt(x, y, cell.boardItem);
                }
                CreateBackgroundImageAt(x, y);
            }
        }
    }

    private BoardItemView CreateBoardItemViewAt(int x, int y, IBoardItem boardItem)
    {
        var boardItemView = boardItemViewFactory.CreateBoardItem(boardItem, GetCellWorldPosition(x, y));
        boardItemViews[(x, y)] = boardItemView as BoardItemView;
        return boardItemView as BoardItemView;
    }

    private void CreateBackgroundImageAt(int x, int y)
    {
        var worldPosition = GetCellWorldPosition(x, y);
        //so that they are always in the background
        worldPosition.z = 100;
        Instantiate(backgroundTilePrefab, worldPosition, Quaternion.identity, transform);
    }

    private Vector3 GetCellWorldPosition(int x, int y)
    {
        //-y on z axis for 2D view
        return new Vector3(x * boardItemSize - xOffset, y * boardItemSize - yOffset, -y);
    }

}
