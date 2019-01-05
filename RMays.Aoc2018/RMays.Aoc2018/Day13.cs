using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day13
    {
        public class Cart
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public Path DirectionFacing { get; set; }
            public bool IsCrashed { get; set; }
            public int nextCartTurn = 0; // 0 = left, 1 = straight, 2 = right

            public Cart()
            {
                IsCrashed = false;
                nextCartTurn = 0;
                // Everything else needs to be set explicitly.
            }

            public void GoStraight()
            {
                switch (DirectionFacing)
                {
                    case Path.North:
                        Row--;
                        break;
                    case Path.South:
                        Row++;
                        break;
                    case Path.East:
                        Col++;
                        break;
                    case Path.West:
                        Col--;
                        break;
                }

                // Change direction, if necessary.
                // I think all this code should be in a 'controller' class.  Hmm.
            }

            public void TurnAtIntersection()
            {
                var currDirections = new Dictionary<Path, int>();
                currDirections.Add(Path.North, 0);
                currDirections.Add(Path.East, 1);
                currDirections.Add(Path.South, 2);
                currDirections.Add(Path.West, 3);
                var turnRightDirections = new List<Path> { Path.East, Path.South, Path.West, Path.North };
                var turnLeftDirections = new List<Path> { Path.West, Path.North, Path.East, Path.South };

                var currIndex = currDirections.Where(x => x.Key == DirectionFacing).First().Value;

                switch (nextCartTurn)
                {
                    case 0: // left
                        DirectionFacing = turnLeftDirections[currIndex];
                        break;
                    case 1: // straight
                        // do nothing!
                        break;
                    case 2: // right
                        DirectionFacing = turnRightDirections[currIndex];
                        break;
                }

                nextCartTurn++;
                if (nextCartTurn >= 3) nextCartTurn -= 3;
            }
        }

        public class Grid
        {
            protected Path[,] pathGrid { get; set; }
            public int Rows { get; set; }
            public int Cols { get; set; }
            public List<Cart> Carts { get; set; }

            public Grid(string input)
            {
                int numRows;
                int numCols;
                var charGrid = GetCharGridFromInput(input, out numRows, out numCols);
                Rows = numRows;
                Cols = numCols;
                InitializePathGrid(charGrid);
            }

            public char[,] GetCharGridFromInput(string input, out int numRows, out int numCols)
            {
                var lines = input.Split(new char[] { '\r', '\n' }).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x).ToList();
                // Turn the lines into a grid of characters.
                numRows = lines.Count;
                numCols = lines[0].Length;

                var grid = new char[numRows, numCols];

                var row = 0;
                foreach (var line in lines)
                {
                    var col = 0;
                    foreach (var cell in line.ToCharArray())
                    {
                        grid[row, col] = cell;
                        col++;
                    }
                    row++;
                }

                return grid;
            }

            private void InitializePathGrid(char[,] charGrid)
            {
                Carts = new List<Cart>();
                pathGrid = new Path[Rows, Cols];
                var cartChars = new Dictionary<char, Path>();
                cartChars.Add('^', Path.North);
                cartChars.Add('v', Path.South);
                cartChars.Add('>', Path.East);
                cartChars.Add('<', Path.West);
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        var currChar = charGrid[row, col];
                        if (cartChars.Keys.Contains(currChar))
                        {
                            Carts.Add(new Cart
                            {
                                Row = row,
                                Col = col,
                                DirectionFacing = cartChars[currChar],
                                IsCrashed = false
                            });
                        }

                        pathGrid[row, col] = GetPathValue(currChar);
                    }
                }

                // Now, fix the unknowns.
                for (int row = 0; row < Rows; row++)
                {
                    for (int col = 0; col < Cols; col++)
                    {
                        var currPathCell = pathGrid[row, col];
                        if ((currPathCell & Path.Unknown) == Path.Unknown)
                        {
                            if (currPathCell == Path.NEorSW)
                            {
                                if (row == 0 || col == Cols - 1)
                                {
                                    pathGrid[row, col] = Path.SouthWest;
                                    continue;
                                }
                                if (col == 0 || row == Rows - 1)
                                {
                                    pathGrid[row, col] = Path.NorthEast;
                                    continue;
                                }
                                if (pathGrid[row + 1, col] == Path.NorthSouth || pathGrid[row + 1, col] == Path.All ||
                                    pathGrid[row, col - 1] == Path.EastWest || pathGrid[row, col - 1] == Path.All)
                                {
                                    pathGrid[row, col] = Path.SouthWest;
                                    continue;
                                }
                                if (pathGrid[row - 1, col] == Path.NorthSouth || pathGrid[row - 1, col] == Path.All ||
                                    pathGrid[row, col + 1] == Path.EastWest || pathGrid[row, col + 1] == Path.All)
                                {
                                    pathGrid[row, col] = Path.NorthEast;
                                    continue;
                                }
                                throw new ApplicationException($"Couldn't decode this ambiguous track.  Cell: {row},{col}");
                            }
                            else if (currPathCell == Path.NWorSE)
                            {
                                if (row == 0 || col == 0)
                                {
                                    pathGrid[row, col] = Path.SouthEast;
                                    continue;
                                }
                                if (col == Cols - 1 || row == Rows - 1)
                                {
                                    pathGrid[row, col] = Path.NorthWest;
                                    continue;
                                }
                                if (pathGrid[row + 1, col] == Path.NorthSouth || pathGrid[row + 1, col] == Path.All ||
                                    pathGrid[row, col + 1] == Path.EastWest || pathGrid[row, col + 1] == Path.All)
                                {
                                    pathGrid[row, col] = Path.SouthEast;
                                    continue;
                                }
                                if (pathGrid[row - 1, col] == Path.NorthSouth || pathGrid[row - 1, col] == Path.All ||
                                    pathGrid[row, col - 1] == Path.EastWest || pathGrid[row, col - 1] == Path.All)
                                {
                                    pathGrid[row, col] = Path.NorthWest;
                                    continue;
                                }
                                throw new ApplicationException($"Couldn't decode this ambiguous track.  Cell: {row},{col}");
                            }
                            else
                            {
                                throw new ApplicationException($"Couldn't decode this ambiguous track.  Cell: {row},{col}");
                            }
                        }
                    }
                }
            }

            private Path GetPathValue(char gridCell)
            {
                Path path = Path.None;
                switch(gridCell)
                {
                    case ' ':
                        path = Path.None;
                        break;
                    case '/':
                        path = Path.NWorSE;
                        break;
                    case '\\':
                        path = Path.NEorSW;
                        break;
                    case '|':
                        path = Path.NorthSouth;
                        break;
                    case '-':
                        path = Path.EastWest;
                        break;
                    case '+':
                        path = Path.All;
                        break;
                    case '>':
                        path = Path.EastWest;
                        break;
                    case '<':
                        path = Path.EastWest;
                        break;
                    case '^':
                        path = Path.NorthSouth;
                        break;
                    case 'v':
                        path = Path.NorthSouth;
                        break;
                    default:
                        throw new ApplicationException($"Invalid character: {gridCell}");
                }
                return path;
            }
        }

        [Flags]
        public enum Path
        {
            None = 0,

            North = 1,
            South = 2,
            East = 4,
            West = 8,

            NorthSouth = 3, //   |
            NorthEast = 5,  //   \
            NorthWest = 9,  //   /
            SouthEast = 6,  //   /
            SouthWest = 10, //   \
            EastWest = 12,  //   -
            All = 15,       //   +

            Unknown = 16,
            NEorSW = 17,   //   \
            NWorSE = 18    //   /
        }

        public string SolveA(string input)
        {
            var myGrid = new Grid(input);

            return "?";
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 456;
        }
    }
}
