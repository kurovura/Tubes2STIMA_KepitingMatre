using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TreasureHunt
{
    public class TreasureHunterDFS
    {
        private char[,] grid;
        private int rows;
        private int cols;
        private int treasureCount;
        private (int row, int col) start;
        private List<(int row, int col)> treasures = new List<(int row, int col)>();
        private string shortestRoute = "";
        private int minSteps = Int32.MaxValue;
        private int nodesChecked = 0;
        private Stopwatch elapsedTime = new Stopwatch();

        public TreasureHunterDFS(char[,] grid)
        {
            this.grid = grid;
            rows = grid.GetLength(0);
            cols = grid.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i, j] == 'K')
                    {
                        start = (i, j);
                    }
                    else if (grid[i, j] == 'T')
                    {
                        treasureCount++;
                        treasures.Add((i, j));
                    }
                }
            }
        }

        public string FindShortestRoute()
        {

            elapsedTime.Start();

            DFS(start.row, start.col, "", new bool[rows, cols], 0);

            elapsedTime.Stop();

            return shortestRoute;
        }

        public int GetNodes()
        {
            return nodesChecked;
        }

        public long GetElapsedTime()
        {
            return elapsedTime.ElapsedMilliseconds;
        }


        private void DFS(int row, int col, string route, bool[,] visited, int steps)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols || visited[row, col] || grid[row, col] == 'X')
                return;

            visited[row, col] = true;
            nodesChecked++;

            if (grid[row, col] == 'T')
                treasureCount--;

            if (treasureCount == 0)
            {
                if (steps < minSteps)
                {
                    minSteps = steps;
                    shortestRoute = route;
                }
                visited[row, col] = false;
                treasureCount++;
                return;
            }

            DFS(row + 1, col, route + "D", visited, steps + 1);
            DFS(row - 1, col, route + "U", visited, steps + 1);
            DFS(row, col + 1, route + "R", visited, steps + 1);
            DFS(row, col - 1, route + "L", visited, steps + 1);

            visited[row, col] = false;
            if (grid[row, col] == 'T')
                treasureCount++;
        }
    }
}