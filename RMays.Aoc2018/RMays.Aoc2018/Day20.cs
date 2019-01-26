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
                        else
                        {
                            sb.Append(cells[row, col] ? '#' : '.');
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
            grid.Move(Direction.North);
            grid.Move(Direction.South);
            grid.Move(Direction.South);
            grid.Move(Direction.East);
            grid.Move(Direction.West);
            grid.Move(Direction.West);

            return 0;
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 0;
        }
    }
}
