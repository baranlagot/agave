using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardLogic
{
    public class Board
    {
        private BoardCell[,] cells;

        public int Width { get; }
        public int Height { get; }

        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            cells = new BoardCell[width, height];
        }

        public BoardCell GetCell(int x, int y) => cells[x, y];
        public void SetCell(int x, int y, BoardCell cell) => cells[x, y] = cell;

        public bool IsValid(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        // Fill with random colors
        public void InitializeRandom(int colorCount = 4)
        {
            Random rnd = new Random();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var color = (BoardItemColor)(rnd.Next(0, colorCount));
                    var cube = new DefaultBoardItem(color);
                    cells[x, y] = new BoardCell(cube, (x, y));
                }
            }
        }

        public void InitializePredefined(BoardItemColor[,] colors)
        {
            if (colors.GetLength(0) != Width || colors.GetLength(1) != Height)
            {
                throw new ArgumentException("Invalid size of the colors array");
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var cube = new DefaultBoardItem(colors[x, y]);
                    cells[x, y] = new BoardCell(cube, (x, y));
                }
            }
        }

        public ShuffleResults ShuffleBoard()
        {
            var oldPositions = new List<(int x, int y)>();
            var newPositions = new List<(int x, int y)>();

            Random rnd = new Random();
            var positions = new HashSet<(int x, int y)>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    positions.Add((x, y));
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var remainingPositions = positions.ToArray();
                    int randomIndex = rnd.Next(0, remainingPositions.Length);
                    var randomPos = remainingPositions[randomIndex];

                    positions.Remove(randomPos);

                    if ((x, y) != randomPos)
                    {
                        oldPositions.Add((x, y));
                        newPositions.Add(randomPos);
                        SwapCells((x, y), randomPos);
                    }
                }
            }
            var isLinked = LinkCheckingSystem.CheckLinks(this);
            if (!isLinked)
            {
                return ShuffleBoard();
            }
            return new ShuffleResults(oldPositions, newPositions);
        }

        private void SwapCells((int x, int y) first, (int x, int y) second)
        {
            var temp = cells[first.x, first.y];
            cells[first.x, first.y] = cells[second.x, second.y];
            cells[second.x, second.y] = temp;
        }

    }
}

public struct ShuffleResults
{
    public List<(int x, int y)> oldPositions;
    public List<(int x, int y)> newPositions;

    public ShuffleResults(List<(int x, int y)> oldPositions, List<(int x, int y)> newPositions)
    {
        this.oldPositions = oldPositions;
        this.newPositions = newPositions;
    }

}