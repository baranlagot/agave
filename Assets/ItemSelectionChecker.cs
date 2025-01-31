using BoardLogic;
using UnityEngine;

public static class ItemSelectionChecker
{
    public static bool CanSelectItem((int x, int y) boardPosition, Board board, BoardCell? lastSelectedCell)
    {
        // Check if the position is valid
        if (!board.IsValid(boardPosition.x, boardPosition.y))
        {
            return false;
        }

        // Check if the cell is empty or the same as the last selected cell
        var cell = board.GetCell(boardPosition.x, boardPosition.y);
        if (cell.IsEmpty || cell.Equals(lastSelectedCell))
        {
            return false;
        }

        // Check if the cell is a neighbour of the last selected cell
        if (lastSelectedCell == null)
        {
            return true;
        }

        // Check if the cell is a neighbour of the last selected cell
        return IsNeighbour(boardPosition, lastSelectedCell.Value) && IsSameColor(cell, lastSelectedCell.Value);
    }

    private static bool IsNeighbour((int x, int y) boardPosition, BoardCell lastSelectedCell)
    {
        return Mathf.Abs(boardPosition.x - lastSelectedCell.BoardPosition.x) <= 1 &&
               Mathf.Abs(boardPosition.y - lastSelectedCell.BoardPosition.y) <= 1;
    }

    private static bool IsSameColor(BoardCell cell1, BoardCell cell2)
    {
        return cell1.boardItem.Color == cell2.boardItem.Color;
    }
}