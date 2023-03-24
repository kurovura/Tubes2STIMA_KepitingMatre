using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureHunt
{
    internal class GridOperations
    {
        public static List<(int, int)> getTreasuresCoord(char[,] inputGrid)
        {
            List<(int, int)> treasuresCoord = new List<(int, int)>();
            for (int i = 0; i < inputGrid.GetLength(0); i++)
            {
                for (int j = 0; j < inputGrid.GetLength(1); j++)
                {
                    if (inputGrid[i, j] == 'T')
                    {
                        treasuresCoord.Add((i, j));
                    }
                }
            }
            return treasuresCoord;
        }

        public static List<(int, int)> getJellyFishCoord(char[,] inputGrid)
        {
            List<(int, int)> jellyfishcoord = new List<(int, int)>();
            for (int i = 0; i < inputGrid.GetLength(0); i++)
            {
                for (int j = 0; j < inputGrid.GetLength(1); j++)
                {
                    if (inputGrid[i, j] == 'X')
                    {
                        jellyfishcoord.Add((i, j));
                    }
                }
            }
            return jellyfishcoord;
        }

        public static (int, int) getStartCoord(char[,] inputGrid)
        {
            for (int i = 0; i < inputGrid.GetLength(0); i++)
            {
                for (int j = 0; j < inputGrid.GetLength(1); j++)
                {
                    if (inputGrid[i, j] == 'K')
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
    }
}
