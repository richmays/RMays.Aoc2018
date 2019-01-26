using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day20
    {
        public class Grid
        {
            private int Rows = 1;
            private int Cols = 1;

            private int RowsGrid => Rows * 2 + 1;
            private int ColsGrid => Cols * 2 + 1;

            private int RowOffset = 0;
            private int ColOffset = 0;

            // The current position of the unit.
            private int CurrRow = 0;
            private int CurrCol = 0;

            private int CurrRowGrid => (CurrRow * 2) + (RowOffset * 2) + 1;
            private int CurrColGrid => (CurrCol * 2) + (ColOffset * 2) + 1;

            private List<Coords> CoordStack;

            /// <summary>
            /// True = wall, False = clear
            /// </summary>
            private bool[,] cells;

            public Grid()
            {
                cells = new bool[RowsGrid, ColsGrid];
                for (var row = 0; row < RowsGrid; row++)
                {
                    for(var col = 0; col < ColsGrid; col++)
                    {
                        cells[row, col] = true;
                    }
                }

                cells[CurrRowGrid, CurrColGrid] = false;
                CoordStack = new List<Coords>();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                for (var row = 0; row < RowsGrid; row++)
                {
                    for (var col = 0; col < ColsGrid; col++)
                    {
                        if (row == CurrRowGrid && col == CurrColGrid)
                        {
                            sb.Append('X');
                        }
                        else if (cells[row, col])
                        {
                            sb.Append('#');
                        }
                        else if (row % 2 == 1 && col % 2 == 1)
                        {
                            sb.Append('.');
                        }
                        else
                        {
                            sb.Append(' ');
                        }
                    }
                    sb.Append(Environment.NewLine);
                }
                sb.Append(Environment.NewLine);
                return sb.ToString();
            }

            public void MoveToCell(int row, int col)
            {
                // We assume that we only move to a cell that's already in the grid.
                CurrRow = row;
                CurrCol = col;
            }

            /// <summary>
            /// Stretch the whole grid one row / col in the given direction.
            /// North / west is harder, because we have to copy the grid.
            /// </summary>
            /// <param name="direction"></param>
            private void Stretch(Direction direction)
            {
                bool[,] temp;
                switch (direction)
                {
                    case Direction.North:
                        temp = new bool[RowsGrid + 2, ColsGrid];

                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                temp[gridRow + 2, gridCol] = cells[gridRow, gridCol];
                            }
                        }

                        for (int gridRow = 0; gridRow < 2; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                temp[gridRow, gridCol] = true;
                            }
                        }

                        Rows++;
                        RowOffset++;

                        cells = new bool[RowsGrid + 2, ColsGrid];
                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                cells[gridRow, gridCol] = temp[gridRow, gridCol];
                            }
                        }

                        break;
                    case Direction.South:
                        temp = new bool[RowsGrid + 2, ColsGrid];

                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                temp[gridRow, gridCol] = cells[gridRow, gridCol];
                            }
                        }

                        for (int gridRow = RowsGrid; gridRow < RowsGrid + 2; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                temp[gridRow, gridCol] = true;
                            }
                        }

                        Rows++;

                        cells = new bool[RowsGrid + 2, ColsGrid];
                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                cells[gridRow, gridCol] = temp[gridRow, gridCol];
                            }
                        }

                        break;
                    case Direction.East:
                        temp = new bool[RowsGrid, ColsGrid + 2];

                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                temp[gridRow, gridCol] = cells[gridRow, gridCol];
                            }
                        }

                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = ColsGrid; gridCol < ColsGrid + 2; gridCol++)
                            {
                                temp[gridRow, gridCol] = true;
                            }
                        }

                        Cols++;

                        cells = new bool[RowsGrid, ColsGrid + 2];
                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                cells[gridRow, gridCol] = temp[gridRow, gridCol];
                            }
                        }

                        break;
                    case Direction.West:
                        temp = new bool[RowsGrid, ColsGrid + 2];

                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                temp[gridRow, gridCol + 2] = cells[gridRow, gridCol];
                            }
                        }

                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < 2; gridCol++)
                            {
                                temp[gridRow, gridCol] = true;
                            }
                        }

                        Cols++;
                        ColOffset++;

                        cells = new bool[RowsGrid, ColsGrid + 2];
                        for (int gridRow = 0; gridRow < RowsGrid; gridRow++)
                        {
                            for (int gridCol = 0; gridCol < ColsGrid; gridCol++)
                            {
                                cells[gridRow, gridCol] = temp[gridRow, gridCol];
                            }
                        }

                        break;
                }
            }

            public void Move(Direction direction)
            {
                // It's OK to go negative.
                switch (direction)
                {
                    case Direction.North:
                        if (CurrRow + RowOffset == 0)
                        {
                            Stretch(direction);
                        }

                        cells[CurrRowGrid - 1, CurrColGrid] = false;
                        cells[CurrRowGrid - 2, CurrColGrid] = false;
                        CurrRow--;
                        break;
                    case Direction.South:
                        if (CurrRow + RowOffset >= (Rows - 1))
                        {
                            Stretch(direction);
                        }

                        cells[CurrRowGrid + 1, CurrColGrid] = false;
                        cells[CurrRowGrid + 2, CurrColGrid] = false;
                        CurrRow++;
                        break;
                    case Direction.East:
                        if (CurrCol + ColOffset >= (Cols - 1))
                        {
                            Stretch(direction);
                        }

                        cells[CurrRowGrid, CurrColGrid + 1] = false;
                        cells[CurrRowGrid, CurrColGrid + 2] = false;
                        CurrCol++;
                        break;
                    case Direction.West:
                        if (CurrCol + ColOffset == 0)
                        {
                            Stretch(direction);
                        }

                        cells[CurrRowGrid, CurrColGrid - 1] = false;
                        cells[CurrRowGrid, CurrColGrid - 2] = false;
                        CurrCol--;
                        break;
                }
            }

            public void PushCoords()
            {
                CoordStack.Add(new Coords(CurrRow, CurrCol));
            }

            public void PopCoords()
            {
                var lastCoords = CoordStack.Last();
                CurrRow = lastCoords.Row;
                CurrCol = lastCoords.Col;
                CoordStack.RemoveAt(CoordStack.Count - 1);
            }

            public void Build(string input)
            {
                // ^ENWWW(NEEE|SSE(EE|N))$
                foreach (var c in input.ToCharArray())
                {
                    switch(c)
                    {
                        case '^':
                            break;
                        case '$':
                            // All done!
                            MoveToCell(0, 0);
                            return;
                        case 'N':
                            Move(Direction.North);
                            break;
                        case 'S':
                            Move(Direction.South);
                            break;
                        case 'E':
                            Move(Direction.East);
                            break;
                        case 'W':
                            Move(Direction.West);
                            break;
                        case '(':
                            PushCoords();
                            break;
                        case ')':
                            PopCoords();
                            break;
                        case '|':
                            PopCoords();
                            PushCoords();
                            break;
                    }
                }
            }

            public int GetLongestPath()
            {
                return -1;
            }
        }

        public enum Direction
        {
            North,
            South,
            East,
            West
        }

        public long SolveA(string input)
        {
            var grid = new Grid();
            grid.Build(input);
            return grid.GetLongestPath();
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 0;
        }
    }
}
