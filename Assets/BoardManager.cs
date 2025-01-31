using System;
using System.Collections;
using System.Collections.Generic;
using BoardLogic;
using UnityEngine;

public class BoardManager : MonoBehaviour, ICommandContext
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

    internal Vector3 GetCellWorldPosition(int x, int y)
    {
        //-y on z axis for 2D view
        return new Vector3(x * boardItemSize - xOffset, y * boardItemSize - yOffset, -y);
    }

    public BoardCell? GetBoardCellFromMousePosition(Vector3 mousePosition)
    {
        var boardPosition = GetClosestCellPosition(mousePosition);
        if (boardPosition.x < 0 || boardPosition.x >= width || boardPosition.y < 0 || boardPosition.y >= height)
        {
            return null;
        }

        return board.GetCell(boardPosition.x, boardPosition.y);
    }

    public Board GetBoard()
    {
        return board;
    }


    public (int x, int y) GetClosestCellPosition(Vector3 worldPos)
    {
        (int, int) closestCell = (-1, -1);
        var minDist = float.MaxValue;
        var threshold = boardItemSize * boardItemSize;

        foreach (var item in boardItemViews)
        {
            var cubePos = item.Value.transform.position;
            var distSqr = ((Vector2)cubePos - (Vector2)worldPos).sqrMagnitude;
            if (distSqr < minDist)
            {
                minDist = distSqr;
                closestCell = item.Key;
            }
        }

        if (minDist > threshold)
        {
            return (-1, -1);
        }

        return closestCell;
    }

    public void ExecuteCommands(List<IBoardCommand> commands)
    {
        foreach (var command in commands)
        {
            command.Execute(this);
        }
    }

    //-----------------------------Command context implementation--------------------------------

    public void DestoryBoardItem((int x, int y) position)
    {
        if (boardItemViews.TryGetValue(position, out var boardItemView))
        {
            boardItemView.Factory.ReleaseObject(boardItemView);
            boardItemViews.Remove(position);
        }
    }

    public void MoveBoardItem((int x, int y) from, (int x, int y) to)
    {
        if (boardItemViews.TryGetValue(from, out var boardItemView))
        {
            var targetPosition = GetCellWorldPosition(to.x, to.y);
            boardItemView.MoveTo(targetPosition);
            boardItemViews.Remove(from);
            boardItemViews[to] = boardItemView;
        }
    }

    public void CreateBoardItem((int x, int y) position, IBoardItem boardItem)
    {
        CreateBoardItemViewAt(position.x, position.y, boardItem);
    }

    public void AddDelay(float seconds)
    {
        //TODO: add delay command
    }
}
