using System.Collections.Generic;
using System.Linq;
using BoardLogic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BoardManager : MonoBehaviour, ICommandContext
{
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private BoardItemView boardItemViewPrefab;
    [SerializeField] private GameObject backgroundTilePrefab;
    [SerializeField] private FeedbackPlayer feedbackPlayer;
    [SerializeField] private ParticleFeedback particleFeedback;

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

    private void ShuffleBoard()
    {
        Debug.Log("Shuffling board");
        var shuffleResults = board.ShuffleBoard();
        ProcessShuffleResults(shuffleResults);
    }

    private void ProcessShuffleResults(ShuffleResults results)
    {
        var moveTime = 0.5f;
        for (int i = 0; i < results.oldPositions.Count; i++)
        {
            var oldPos = results.oldPositions[i];
            var newPos = results.newPositions[i];
            if (boardItemViews.TryGetValue(oldPos, out var boardItemView))
            {
                var newBoardItemView = boardItemViews[newPos];
                boardItemViews.Remove(oldPos);
                boardItemViews.Remove(newPos);
                boardItemViews[oldPos] = newBoardItemView;
                boardItemViews[newPos] = boardItemView;
                board.SetCell(oldPos.Item1, oldPos.Item2, new BoardCell(newBoardItemView.BoardItem, oldPos));
                board.SetCell(newPos.Item1, newPos.Item2, new BoardCell(boardItemView.BoardItem, newPos));
                boardItemView.MoveTo(GetCellWorldPosition(newPos.Item1, newPos.Item2), moveTime);
                newBoardItemView.MoveTo(GetCellWorldPosition(oldPos.Item1, oldPos.Item2), moveTime);
            }
        }
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
        var threshold = boardItemSize * boardItemSize * 0.5f;

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

    public void SetSelectedItems(Stack<BoardCell> selectedItems)
    {
        foreach (var item in selectedItems)
        {
            var position = item.BoardPosition;
            if (boardItemViews.TryGetValue(position, out var boardItemView))
            {
                boardItemView.SetSelected();
            }
        }
    }

    public void ClearSelectedItems()
    {
        foreach (var item in boardItemViews)
        {
            item.Value.SetDeselected();
        }
    }

    public static Color GetColorFromBoardItem(IBoardItem boardItem)
    {
        switch (boardItem.Color)
        {
            case BoardItemColor.Blue:
                return Color.blue;
            case BoardItemColor.Green:
                return Color.green;
            case BoardItemColor.Red:
                return Color.red;
            case BoardItemColor.Yellow:
                return Color.yellow;
            default:
                return Color.white;
        }
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
            board.SetCell(position.x, position.y, new BoardCell(null, position));
            particleFeedback.feedbackColor = GetColorFromBoardItem(boardItemView.BoardItem);
            feedbackPlayer.Play(boardItemView.transform.position);
            boardItemView.Factory.ReleaseObject(boardItemView);
            boardItemViews.Remove(position);
        }
    }

    public void MoveBoardItem((int x, int y) from, (int x, int y) to)
    {
        if (boardItemViews.TryGetValue(from, out var boardItemView))
        {
            var targetPosition = GetCellWorldPosition(to.x, to.y);
            boardItemView.FallTo(targetPosition, 0f);
            boardItemViews.Remove(from);
            boardItemViews[to] = boardItemView;
            board.SetCell(to.x, to.y, new BoardCell(boardItemView.BoardItem, to));
        }
    }

    public void CreateBoardItem((int x, int y) position, IBoardItem boardItem)
    {
        CreateBoardItemViewAt(position.x, position.y, boardItem);
    }


    public void RefillBoard((int x, int y) finalPos, IBoardItem boardItem, float spawnY)
    {
        var boardItemView = CreateBoardItemViewAt(finalPos.x, finalPos.y, boardItem);
        boardItemView.SetPosition(spawnY + finalPos.y * boardItemSize);
        boardItemView.FallTo(GetCellWorldPosition(finalPos.x, finalPos.y), 0f);

        //after every refill, check if no more matches are possible
        var anyLinksRemaining = LinkCheckingSystem.CheckLinks(board);
        if (!anyLinksRemaining)
        {
            ShuffleBoard();
        }
    }

}