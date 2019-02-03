using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day22
    {
        public long[,] GeoIndexes { get; set; }
        public int[,] Erosions { get; set; }
        public long MaxX { get; set; }
        public long MaxY { get; set; }
        public long Depth { get; set; }
        
        public long GetGeoIndex(long x, long y)
        {
            if (x == 0 && y == 0)
            {
                return 0;
            }
            if (y == 0)
            {
                return 16807 * x;
            }
            if (x == 0)
            {
                return 48271 * y;
            }
            return Erosions[x - 1, y] * Erosions[x, y - 1];
        }

        public int GetErosionLevel(long x, long y)
        {
            return (int)((GetGeoIndex(x, y) + Depth) % 20183);
        }

        public void Build(string input)
        {
            // depth: 510
            // target: 10,10

            var lines = Parser.TokenizeLines(input);
            Depth = long.Parse(lines[0].Split(' ')[1]);
            MaxX = long.Parse(lines[1].Split(' ')[1].Split(',')[0]);
            MaxY = long.Parse(lines[1].Split(' ')[1].Split(',')[1]);

            GeoIndexes = new long[MaxX + 11, MaxY + 11];
            Erosions = new int[MaxX + 11, MaxY + 11];

            for (long x = 0; x <= MaxX + 10; x++)
            {
                for (long y = 0; y <= MaxY + 10; y++)
                {
                    GeoIndexes[x, y] = GetGeoIndex(x, y);
                    Erosions[x, y] = GetErosionLevel(x, y);
                }
            }
        }

        #region Part A

        public long GetRiskLevel()
        {
            var riskLevel = 0;
            for (long x = 0; x <= MaxX; x++)
            {
                for (long y = 0; y <= MaxY; y++)
                {
                    if (x != MaxX || y != MaxY)
                    {
                        riskLevel += Erosions[x, y] % 3;
                    }
                }
            }

            return riskLevel;
        }

        #endregion

        public long SolveA(string input)
        {
            Build(input);

            return GetRiskLevel();
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 0;
        }
    }
}
