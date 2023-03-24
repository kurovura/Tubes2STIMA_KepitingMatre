using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureHunt
{
    internal class GetFile
    {
        public static char[,] GetGrid(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            char[,] grid = new char[lines.Length, lines[0].Split(' ').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitLine = lines[i].Split(' ');
                for (int j = 0; j < splitLine.Length; j++)
                {
                    grid[i, j] = char.Parse(splitLine[j]);
                }
            }
            return grid;
        }
    }
}
