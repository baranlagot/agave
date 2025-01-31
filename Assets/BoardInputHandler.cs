using System.Collections.Generic;
using BoardLogic;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    private Stack<BoardCell> selectedItems = new Stack<BoardCell>();
    private Stack<Vector3> selectedCellWorldPositions = new Stack<Vector3>();
    private readonly int minSelectionCount = 3;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var boardCell = boardManager.GetBoardCellFromMousePosition(mousePos);
            Debug.Log("VSVV");
            if (!boardCell.HasValue)
            {
                return;
            }
            Debug.Log("ASDASD");

            var boardCellValue = boardCell.Value;
            BoardCell? lastSelectedCell = selectedItems.Count > 0 ? selectedItems.Peek() : (BoardCell?)null;
            if (ItemSelectionChecker.CanSelectItem(boardCellValue.BoardPosition, boardManager.GetBoard(), lastSelectedCell) && !selectedItems.Contains(boardCellValue))
            {
                selectedItems.Push(boardCellValue);
                selectedCellWorldPositions.Push(boardManager.GetCellWorldPosition(boardCellValue.BoardPosition.x, boardCellValue.BoardPosition.y));
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectedItems.Count >= minSelectionCount)
            {
                var commands = SelectionResultConverter.ConvertToSelectionResult(selectedItems);
                boardManager.ExecuteCommands(commands);
            }
            selectedItems.Clear();
            selectedCellWorldPositions.Clear();
        }
        Debug.Log("Selected items: " + selectedItems.Count);
    }

    private void LateUpdate()
    {
        LineDrawer.DrawLine(selectedCellWorldPositions);
    }

}