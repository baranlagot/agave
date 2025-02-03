using System.Collections.Generic;
using BoardLogic;

public static class LinkCheckingSystem
{
    public static bool CheckLinks(Board board)
    {
        var linkedMinLimit = 3;

        HashSet<(int, int)> visited = new HashSet<(int, int)>(); //visited cells

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                if (visited.Contains((x, y)))
                {
                    continue;
                }

                var cell = board.GetCell(x, y);
                if (cell.IsEmpty)
                {
                    continue;
                }

                var connected = GetConnectedCluster(board, (x, y));
                if (connected.Count >= linkedMinLimit)
                {
                    return true;
                }

                foreach (var c in connected)
                {
                    visited.Add(c);
                }
            }
        }

        return false;
    }

    private static List<(int x, int y)> GetConnectedCluster(Board board, (int x, int y) start)
    {
        var connected = new List<(int, int)>();
        var visited = new bool[board.Width, board.Height];
        var stack = new Stack<(int, int)>();

        stack.Push((start.x, start.y));
        visited[start.x, start.y] = true;

        int[][] dirs = new int[][]
        {
                new[]{1,0}, new[]{-1,0},
                new[]{0,1}, new[]{0,-1},
                new[]{1,1}, new[]{-1,-1},
                new[]{1,-1}, new[]{-1,1}
        };

        while (stack.Count > 0)
        {
            var (cx, cy) = stack.Pop();
            connected.Add((cx, cy));

            foreach (var d in dirs)
            {
                int nx = cx + d[0];
                int ny = cy + d[1];
                if (board.IsValid(nx, ny) && !visited[nx, ny])
                {
                    var neighbor = board.GetCell(nx, ny);
                    if (!neighbor.IsEmpty && neighbor.boardItem.Color == board.GetCell(cx, cy).boardItem.Color)
                    {
                        visited[nx, ny] = true;
                        stack.Push((nx, ny));
                    }
                }
            }
        }
        return connected;
    }
}

