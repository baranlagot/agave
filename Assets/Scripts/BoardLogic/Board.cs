using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardLogic
{

    /// <summary>
    /// Represents the game board.
    /// </summary>
    public class Board
    {
        private readonly BoardCell[,] cells;
        private Random random;

        public int Width { get; }
        public int Height { get; }

        private const int DEFAULT_COLOR_COUNT = 4;
        private const int MIN_BOARD_SIZE = 3;

        /// <summary>
        /// Initializes a new instance of the Board class.
        /// </summary>
        /// <param name="width">The width of the board.</param>
        /// <param name="height">The height of the board.</param>
        public Board(int width, int height)
        {
            ValidateBoardDimensions(width, height);

            Width = width;
            Height = height;
            cells = new BoardCell[width, height];
            random = new Random();
        }

        /// <summary>
        /// Gets the cell at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <returns>The cell at the specified coordinates.</returns>   
        public BoardCell GetCell(int x, int y)
        {
            ValidatePosition(x, y);
            return cells[x, y];
        }

        /// <summary>
        /// Sets the cell at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <param name="cell">The cell to set.</param>
        public void SetCell(int x, int y, BoardCell cell)
        {
            ValidatePosition(x, y);
            cells[x, y] = cell;
        }

        /// <summary>
        /// Checks if the specified position is valid.
        /// </summary>
        /// <param name="x">The x-coordinate of the position.</param>
        /// <param name="y">The y-coordinate of the position.</param>
        /// <returns>True if the position is valid, false otherwise.</returns>
        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// Initializes the board with random colors.
        /// </summary>
        /// <param name="colorCount">The number of colors to use.</param>
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

        /// <summary>
        /// Shuffles the board.
        /// </summary>
        /// <returns>The results of the shuffle.</returns>
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