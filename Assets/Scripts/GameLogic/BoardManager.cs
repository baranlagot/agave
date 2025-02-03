using System;
using System.Collections;
using System.Collections.Generic;
using BoardLogic;
using UnityEngine;

public class BoardManager : MonoBehaviour, ICommandContext
{
    [SerializeField] private GameData gameData;
    [SerializeField] private BoardItemView boardItemViewPrefab;
    [SerializeField] private GameObject backgroundTilePrefab;
    [SerializeField] private FeedbackPlayer feedbackPlayer;
    [SerializeField] private ParticleFeedback particleFeedback;

    private readonly Dictionary<(int, int), BoardItemView> boardItemViews = new Dictionary<(int, int), BoardItemView>();
    private IBoardItemViewFactory boardItemViewFactory;
    private Board board;

    // Configuration
    private float xOffset => (width * GameConstants.BOARD_ITEM_SIZE) / 2f - GameConstants.BOARD_ITEM_SIZE / 2f;
    private float yOffset => (height * GameConstants.BOARD_ITEM_SIZE) / 2f - GameConstants.BOARD_ITEM_SIZE / 2f;
    private int width => gameData.boardWidth;
    private int height => gameData.boardHeight;

    #region Initialization

    private void Awake()
    {
        InitializeBoard();
        InitializeVisuals();
    }

    private void InitializeBoard()
    {
        board = new Board(width, height);
        board.InitializeRandom();
    }

    private void InitializeVisuals()
    {
        boardItemViewFactory = new BoardItemViewFactory(boardItemViewPrefab, width * height, transform);
        CreateVisualsFromBoard();
    }

    #endregion

    #region Board Visualization

    private void CreateVisualsFromBoard()
    {
        boardItemViews.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                CreateBackgroundImageAt(x, y);
                var cell = board.GetCell(x, y);
                if (!cell.IsEmpty)
                {
                    CreateBoardItemViewAt(x, y, cell.boardItem);
                }
            }
        }
    }

    private BoardItemView CreateBoardItemViewAt(int x, int y, IBoardItem boardItem)
    {
        var position = GetCellWorldPosition(x, y);
        var boardItemView = boardItemViewFactory.CreateBoardItem(boardItem, position) as BoardItemView;
        boardItemViews[(x, y)] = boardItemView;
        return boardItemView;
    }

    private void CreateBackgroundImageAt(int x, int y)
    {
        var worldPosition = GetCellWorldPosition(x, y);
        Instantiate(backgroundTilePrefab, worldPosition, Quaternion.identity, transform);
    }

    internal Vector3 GetCellWorldPosition(int x, int y)
    {
        return new Vector3(x * GameConstants.BOARD_ITEM_SIZE - xOffset, y * GameConstants.BOARD_ITEM_SIZE - yOffset, -y);
    }

    #endregion

    #region Board State Management

    public Board GetBoard() => board;

    public BoardCell? GetBoardCellFromMousePosition(Vector3 mousePosition)
    {
        var boardPosition = GetClosestCellPosition(mousePosition);
        if (!IsValidPosition(boardPosition))
        {
            return null;
        }
        return board.GetCell(boardPosition.x, boardPosition.y);
    }

    private bool IsValidPosition((int x, int y) position)
    {
        return position.x >= 0 && position.x < width &&
               position.y >= 0 && position.y < height;
    }

    public (int x, int y) GetClosestCellPosition(Vector3 worldPos)
    {
        var threshold = GameConstants.BOARD_ITEM_SIZE * GameConstants.BOARD_ITEM_SIZE * GameConstants.SELECTION_THRESHOLD;
        var closestCell = (-1, -1);
        var minDist = float.MaxValue;

        foreach (var item in boardItemViews)
        {
            var distSqr = ((Vector2)item.Value.transform.position - (Vector2)worldPos).sqrMagnitude;
            if (distSqr < minDist)
            {
                minDist = distSqr;
                closestCell = item.Key;
            }
        }

        return minDist > threshold ? (-1, -1) : closestCell;
    }

    #endregion

    #region Selection Handling

    public void SetSelectedItems(Stack<BoardCell> selectedItems)
    {
        foreach (var item in selectedItems)
        {
            if (boardItemViews.TryGetValue(item.BoardPosition, out var view))
            {
                view.SetSelected();
            }
        }
    }

    public void ClearSelectedItems()
    {
        foreach (var view in boardItemViews.Values)
        {
            view.SetDeselected();
        }
    }

    #endregion

    #region Command Execution

    public void ExecuteCommands(List<IBoardCommand> commands)
    {
        foreach (var command in commands)
        {
            command.Execute(this);
        }
    }

    public void CreateBoardItem((int x, int y) position, IBoardItem boardItem)
    {
        CreateBoardItemViewAt(position.x, position.y, boardItem);
    }

    public void DestoryBoardItem((int x, int y) position)
    {
        if (boardItemViews.TryGetValue(position, out var view))
        {
            board.SetCell(position.x, position.y, new BoardCell(null, position));
            PlayDestroyFeedback(view);
            view.Factory.ReleaseObject(view);
            boardItemViews.Remove(position);
        }
    }

    public void MoveBoardItem((int x, int y) from, (int x, int y) to)
    {
        if (boardItemViews.TryGetValue(from, out var view))
        {
            view.FallTo(GetCellWorldPosition(to.x, to.y), 0f);
            boardItemViews.Remove(from);
            boardItemViews[to] = view;
            board.SetCell(to.x, to.y, new BoardCell(view.BoardItem, to));
        }
    }

    public void RefillBoard((int x, int y) finalPos, IBoardItem boardItem, float spawnY)
    {
        var view = CreateBoardItemViewAt(finalPos.x, finalPos.y, boardItem);
        view.SetPosition(spawnY + finalPos.y * GameConstants.BOARD_ITEM_SIZE);
        view.FallTo(GetCellWorldPosition(finalPos.x, finalPos.y), 0f);
    }

    public void CheckLinksRemaining()
    {
        if (!LinkCheckingSystem.CheckLinks(board))
        {
            ShuffleBoard();
        }
    }

    #endregion

    #region Feedback and Effects

    private void PlayDestroyFeedback(BoardItemView view)
    {
        particleFeedback.feedbackColor = GetColorFromBoardItem(view.BoardItem);
        feedbackPlayer.Play(view.transform.position);
    }

    public static Color GetColorFromBoardItem(IBoardItem boardItem) => boardItem.Color switch
    {
        BoardItemColor.Blue => Color.blue,
        BoardItemColor.Green => Color.green,
        BoardItemColor.Red => Color.red,
        BoardItemColor.Yellow => Color.yellow,
        _ => Color.white
    };

    private void ShuffleBoard()
    {
        StartCoroutine(DelayRoutine(2f, () =>
        {
            var shuffleResults = board.ShuffleBoard();
            ProcessShuffleResults(shuffleResults);
        }));
    }

    private void ProcessShuffleResults(ShuffleResults results)
    {
        var moveTime = 0.5f;
        for (int i = 0; i < results.oldPositions.Count; i++)
        {
            var oldPos = results.oldPositions[i];
            var newPos = results.newPositions[i];
            if (boardItemViews.TryGetValue(oldPos, out var oldView))
            {
                var newView = boardItemViews[newPos];
                UpdateBoardItemPositions(oldPos, newPos, oldView, newView, moveTime);
            }
        }
    }

    private void UpdateBoardItemPositions((int, int) oldPos, (int, int) newPos,
        BoardItemView oldView, BoardItemView newView, float moveTime)
    {
        boardItemViews.Remove(oldPos);
        boardItemViews.Remove(newPos);
        boardItemViews[oldPos] = newView;
        boardItemViews[newPos] = oldView;

        board.SetCell(oldPos.Item1, oldPos.Item2, new BoardCell(newView.BoardItem, oldPos));
        board.SetCell(newPos.Item1, newPos.Item2, new BoardCell(oldView.BoardItem, newPos));

        oldView.MoveTo(GetCellWorldPosition(newPos.Item1, newPos.Item2), moveTime);
        newView.MoveTo(GetCellWorldPosition(oldPos.Item1, oldPos.Item2), moveTime);
    }

    private IEnumerator DelayRoutine(float delayAmount, Action action)
    {
        yield return new WaitForSeconds(delayAmount);
        action?.Invoke();
    }


    #endregion
}