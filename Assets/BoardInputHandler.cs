using System.Collections.Generic;
using BoardLogic;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    private Stack<BoardCell> selectedItems = new Stack<BoardCell>();
    private Stack<Vector3> selectedCellWorldPositions = new Stack<Vector3>();
    private readonly int minSelectionCount = 3;
    private Color selectionColor = Color.red;

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
            if (ItemSelectionChecker.CanSelectItem(boardCellValue.BoardPosition, boardManager.GetBoard(), lastSelectedCell) && !selectedItems.Contains(boardCellValue))
            {
                selectionColor = BoardManager.GetColorFromBoardItem(boardCellValue.boardItem);
                selectedItems.Push(boardCellValue);
                selectedCellWorldPositions.Push(boardManager.GetCellWorldPosition(boardCellValue.BoardPosition.x, boardCellValue.BoardPosition.y));
            }
            else if (selectedItems.Contains(boardCellValue))
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
            if (selectedItems.Count >= minSelectionCount)
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
        //convert selected items to commands, first execute deletion commands
        var commands = SelectionResultConverter.ConvertToSelectionResult(selectedItems);
        boardManager.ExecuteCommands(commands);

        var gravityCommands = GravitySystem.ApplyGravity(boardManager.GetBoard());
        boardManager.ExecuteCommands(gravityCommands);

        var refillCommands = RefillSystem.ApplyRefill(boardManager.GetBoard());
        boardManager.ExecuteCommands(refillCommands);

    }

}