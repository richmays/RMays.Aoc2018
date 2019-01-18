using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day17
    {
        public class Grid
        {
            protected Spot[,] spotGrid { get; set; }

            public int Rows { get; set; }
            public int Cols { get; set; }

            public int ColOffset { get; set; }

            public Grid(string input)
            {
                ReadGrid(input);
            }

            internal class LineData
            {
                public char First { get; set; }
                public int FirstMag { get; set; }
                public char Second { get; set; }
                public int SecondMagLow { get; set; }
                public int SecondMagHi { get; set; }
            }

            private void ReadGrid(string input)
            {
                // x=495, y=2..7
                // y=7, x=495..501

                // y is row
                // x is col
                var lines = Parser.TokenizeLines(input);
                var lineData = new List<LineData>();
                foreach(var line in lines)
                {
                    var chunks = line.Split(',');
                    var newLineData = new LineData
                    {
                        First = chunks[0][0],
                        FirstMag = int.Parse(chunks[0].Substring(2)),
                        Second = chunks[1][1],
                        SecondMagLow = int.Parse(chunks[1].Split('.')[0].Substring(3)),
                        SecondMagHi = int.Parse(chunks[1].Split('.')[2])
                    };

                    lineData.Add(newLineData);
                }

                var minCol = Math.Min(
                    lineData.Where(x => x.First == 'x').Min(x => x.FirstMag),
                    lineData.Where(x => x.Second == 'x').Min(x => x.SecondMagLow));
                var maxCol = Math.Max(
                    lineData.Where(x => x.First == 'x').Max(x => x.FirstMag),
                    lineData.Where(x => x.Second == 'x').Max(x => x.SecondMagHi));
                var minRow = Math.Min(
                    lineData.Where(x => x.First == 'y').Min(x => x.FirstMag),
                    lineData.Where(x => x.Second == 'y').Min(x => x.SecondMagLow));
                var maxRow = Math.Max(
                    lineData.Where(x => x.First == 'y').Max(x => x.FirstMag),
                    lineData.Where(x => x.Second == 'y').Max(x => x.SecondMagHi));

                this.ColOffset = minCol - 1;
                this.Rows = maxRow + 1;
                this.Cols = maxCol - minCol + 3;

                spotGrid = new Spot[Rows, Cols];

                foreach (var line in lineData)
                {
                    int row;
                    int col;
                    if (line.First == 'x')
                    {
                        col = line.FirstMag;
                        for (row = line.SecondMagLow; row <= line.SecondMagHi; row++)
                        {
                            spotGrid[row, col - ColOffset] = Spot.Wall;
                        }
                    }
                    else
                    {
                        row = line.FirstMag;
                        for (col = line.SecondMagLow; col <= line.SecondMagHi; col++)
                        {
                            spotGrid[row, col - ColOffset] = Spot.Wall;
                        }
                    }
                }
            }

            public enum Spot
            {
                Space = 0,
                Wall = 1,
                Water = 2
            }

            public override string ToString()
            {
                var toReturn = new StringBuilder();

                // Print row 0
                for (var col = 0; col < Cols; col++)
                {
                    if (col + ColOffset == 500)
                    {
                        toReturn.Append('+');
                    }
                    else
                    {
                        toReturn.Append(GetCharFromSpot(spotGrid[0, col]));
                    }
                }
                toReturn.AppendLine();

                // Print the rest of the rows.
                for (var row = 1; row < Rows; row++)
                {
                    for (var col = 0; col < Cols; col++)
                    {
                        toReturn.Append(GetCharFromSpot(spotGrid[row, col]));
                    }
                    toReturn.AppendLine();
                }
                return toReturn.ToString();
            }

            private char GetCharFromSpot(Spot spot)
            {
                switch(spot)
                {
                    case Spot.Space:
                        return '.';
                    case Spot.Wall:
                        return 'X';
                    case Spot.Water:
                        return '~';
                }
                return '?';
            }
        }

        public long SolveA(string input)
        {

            Grid grid = new Grid(input);
            var lines = Parser.TokenizeLines(input);




            return 0;
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 0;
        }
    }
}
