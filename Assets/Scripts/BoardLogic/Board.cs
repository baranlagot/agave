using System;

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
    }
}