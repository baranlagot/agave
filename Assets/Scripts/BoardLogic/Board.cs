using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardLogic
{
    public class Board
    {
        private readonly BoardCell[,] cells;
        private Random random;

        public int Width { get; }
        public int Height { get; }

        private const int DEFAULT_COLOR_COUNT = 4;
        private const int MIN_BOARD_SIZE = 3;

        public Board(int width, int height)
        {
            ValidateBoardDimensions(width, height);

            Width = width;
            Height = height;
            cells = new BoardCell[width, height];
            random = new Random();
        }

        public BoardCell GetCell(int x, int y)
        {
            ValidatePosition(x, y);
            return cells[x, y];
        }

        public void SetCell(int x, int y, BoardCell cell)
        {
            ValidatePosition(x, y);
            cells[x, y] = cell;
        }

        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        // Fill with random colors
        public void InitializeRandom(int colorCount = DEFAULT_COLOR_COUNT)
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

        public ShuffleResults ShuffleBoard()
        {
            var oldPositions = new List<(int x, int y)>();
            var newPositions = new List<(int x, int y)>();
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
                    int randomIndex = random.Next(0, remainingPositions.Length);
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

        private void ValidateBoardDimensions(int width, int height)
        {
            if (width < MIN_BOARD_SIZE || height < MIN_BOARD_SIZE)
            {
                throw new ArgumentException(
                    $"Board dimensions must be at least {MIN_BOARD_SIZE}x{MIN_BOARD_SIZE}. " +
                    $"Provided: {width}x{height}");
            }
        }

        private void ValidateColorCount(int colorCount)
        {
            if (colorCount <= 1)
            {
                throw new ArgumentException(
                    $"Color count must be greater than 1. Provided: {colorCount}");
            }
        }

        private void ValidatePosition(int x, int y)
        {
            if (!IsValidPosition(x, y))
            {
                throw new ArgumentOutOfRangeException(
                    $"Position ({x},{y}) is outside board boundaries of {Width}x{Height}");
            }
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