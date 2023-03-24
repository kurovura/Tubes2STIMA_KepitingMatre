using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TreasureHunt
{
    public class TreasureHunterDFS
    {
        private char[,] grid;
        private bool[,] visited;
        private int treasureCount;
        private int startX, startY;
        private int[] dx = new int[] { 0, 1, 0, -1 }; // right, down, left, up
        private int[] dy = new int[] { 1, 0, -1, 0 }; // right, down, left, up
        public int countNodes = 0;
        public Stopwatch elapsedTime = new Stopwatch();

        public TreasureHunterDFS(char[,] inputGrid)
        {
            grid = inputGrid;
            visited = new bool[grid.GetLength(0), grid.GetLength(1)];
            treasureCount = 0;
            startX = -1;
            startY = -1;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == 'K')
                    {
                        startX = i;
                        startY = j;
                    }
                    else if (grid[i, j] == 'T')
                    {
                        treasureCount++;
                    }
                }
            }
        }

        public string FindShortestRoute()
        {
            string shortestRoute = "";
            int shortestSteps = int.MaxValue;
            elapsedTime.Start();
            DFS(startX, startY, "", 0, ref shortestRoute, ref shortestSteps, ref countNodes);
            elapsedTime.Stop();
            return shortestRoute;
        }

        private void DFS(int x, int y, string route, int steps, ref string shortestRoute, ref int shortestSteps, ref int nodes)
        {
            if (treasureCount == 0)
            {
                if (steps < shortestSteps)
                {
                    shortestRoute = route;
                    shortestSteps = steps;
                }
                return;
            }
            visited[x, y] = true;
            nodes++;
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];
                if (nx >= 0 && nx < grid.GetLength(0) && ny >= 0 && ny < grid.GetLength(1) && !visited[nx, ny] && grid[nx, ny] != 'X')
                {
                    int treasureCountBefore = treasureCount;
                    if (grid[nx, ny] == 'T')
                    {
                        treasureCount--;
                    }
                    DFS(nx, ny, route + GetDirection(i), steps + 1, ref shortestRoute, ref shortestSteps, ref nodes);
                    if (grid[nx, ny] == 'T')
                    {
                        treasureCount++;
                    }
                    treasureCount = treasureCountBefore;
                }
            }
            visited[x, y] = false;
        }

        private char GetDirection(int i)
        {
            if (i == 0)
            {
                return 'R';
            }
            else if (i == 1)
            {
                return 'D';
            }
            else if (i == 2)
            {
                return 'L';
            }
            else
            {
                return 'U';
            }
        }
    }
}