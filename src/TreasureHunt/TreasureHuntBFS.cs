using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureHunt
{
    internal class TreasureHunterBFS
    {
        private char[,] grid;
        private int startX, startY;
        private List<Tuple<int, int>> treasures = new List<Tuple<int, int>>();
        private Queue<Node> queue = new Queue<Node>();
        private bool[,,] visited;
        public Stopwatch elapsedTime = new Stopwatch();
        public int countNodes { get; private set; } = 0;

        public TreasureHunterBFS(char[,] grid)
        {
            this.grid = grid;
            visited = new bool[grid.GetLength(0), grid.GetLength(1), 1 << (grid.GetLength(0) * grid.GetLength(1))];

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
                        treasures.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
        }

        public string FindShortestRoute()
        {
            elapsedTime.Start();
            queue.Enqueue(new Node(startX, startY, "", 0));
            visited[startX, startY, 0] = true;

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                if (IsFinalState(node))
                {
                    elapsedTime.Stop();
                    return node.path;
                }

                List<Node> neighbors = GetNeighbors(node);
                foreach (Node neighbor in neighbors)
                {
                    if (!visited[neighbor.x, neighbor.y, neighbor.collectedTreasures])
                    {
                        visited[neighbor.x, neighbor.y, neighbor.collectedTreasures] = true;
                        queue.Enqueue(neighbor);
                        countNodes++;
                    }
                }
            }
            elapsedTime.Stop();
            return "No route found";
        }

        private bool IsFinalState(Node node)
        {
            return node.collectedTreasures == (1 << treasures.Count) - 1;
        }

        private List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            //up
            if (node.x > 0 && grid[node.x - 1, node.y] != 'X')
            {
                neighbors.Add(new Node(node.x - 1, node.y, node.path + "U", UpdateCollectedTreasures(node, node.x - 1, node.y)));
            }

            //down
            if (node.x < grid.GetLength(0) - 1 && grid[node.x + 1, node.y] != 'X')
            {
                neighbors.Add(new Node(node.x + 1, node.y, node.path + "D", UpdateCollectedTreasures(node, node.x + 1, node.y)));
            }

            //left
            if (node.y > 0 && grid[node.x, node.y - 1] != 'X')
            {
                neighbors.Add(new Node(node.x, node.y - 1, node.path + "L", UpdateCollectedTreasures(node, node.x, node.y - 1)));
            }

            //right
            if (node.y < grid.GetLength(1) - 1 && grid[node.x, node.y + 1] != 'X')
            {
                neighbors.Add(new Node(node.x, node.y + 1, node.path + "R", UpdateCollectedTreasures(node, node.x, node.y + 1)));
            }

            return neighbors;
        }

        private int UpdateCollectedTreasures(Node node, int x, int y)
        {
            int collectedTreasures = node.collectedTreasures;

            for (int i = 0; i < treasures.Count; i++)
            {
                if (treasures[i].Item1 == x && treasures[i].Item2 == y)
                {
                    collectedTreasures |= (1 << i);
                }
            }

            return collectedTreasures;
        }
    }

    public class Node
    {
        public int x, y, collectedTreasures;
        public string path;

        public Node(int x, int y, string path, int collectedTreasures)
        {
            this.x = x;
            this.y = y;
            this.path = path;
            this.collectedTreasures = collectedTreasures;
        }
    }
}
