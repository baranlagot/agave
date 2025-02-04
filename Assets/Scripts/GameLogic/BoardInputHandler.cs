using System.Collections.Generic;
using BoardLogic;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private GameData gameData;

    private Stack<BoardCell> selectedItems = new Stack<BoardCell>();
    private Stack<Vector3> selectedCellWorldPositions = new Stack<Vector3>();
    private Color selectionColor = Color.red;

    private int moveCount;
    private int scoreCount;

    private void Awake()
    {
        scoreCount = 0;
        moveCount = gameData.moveCount;
    }

    private void Start()
    {
        EventManager.Publish<OnMoveChangedEvent>(new OnMoveChangedEvent(moveCount, scoreCount));
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var boardCell = boardManager.GetBoardCellFromMousePosition(mousePos);
            if (!boardCell.HasValue)
            {
                return;
            }

            var boardCellValue = boardCell.Value;
            BoardCell? lastSelectedCell = selectedItems.Count > 0 ? selectedItems.Peek() : (BoardCell?)null;

            if (ItemSelectionChecker.CanSelectItem(boardCellValue.BoardPosition, boardManager.GetBoard(), lastSelectedCell) && !selectedItems.Contains(boardCellValue))//check if the cell can be selected && the cell is not already selected
            {
                selectionColor = BoardManager.GetColorFromBoardItem(boardCellValue.boardItem);
                selectedItems.Push(boardCellValue); //add the cell to the selected items stack
                selectedCellWorldPositions.Push(boardManager.GetCellWorldPosition(boardCellValue.BoardPosition.x, boardCellValue.BoardPosition.y)); //add the cell's world position to the selected cell world positions stack
            }
            else if (selectedItems.Contains(boardCellValue)) //if the cell is already selected
            {
                boardManager.ClearSelectedItems();
                if (!selectedItems.Peek().Equals(boardCellValue))
                {
                    selectedItems.Pop();
                    selectedCellWorldPositions.Pop();
                }
            }
            boardManager.SetSelectedItems(selectedItems);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedItems.Count >= GameConstants.MIN_SELECTION_COUNT)//if the number of selected items is greater than the minimum selection count
            {
                OnSelectionValid();
            }
            boardManager.ClearSelectedItems();
            selectedItems.Clear();
            selectedCellWorldPositions.Clear();
        }
    }

    private void LateUpdate()
    {
        LineDrawer.DrawLine(selectedCellWorldPositions, selectionColor);
    }

    private void OnSelectionValid()
    {
        moveCount--;
        scoreCount += selectedItems.Count;

        //convert selected items to commands, first execute deletion commands
        var commands = SelectionResultConverter.ConvertToSelectionResult(selectedItems);
        boardManager.ExecuteCommands(commands);

        //apply gravity to the board
        var gravityCommands = GravitySystem.ApplyGravity(boardManager.GetBoard());
        boardManager.ExecuteCommands(gravityCommands);

        //refill the board
        var refillCommands = RefillSystem.ApplyRefill(boardManager.GetBoard());
        boardManager.ExecuteCommands(refillCommands);

        //update the move count and score
        EventManager.Publish<OnMoveChangedEvent>(new OnMoveChangedEvent(moveCount, scoreCount));
    }

}