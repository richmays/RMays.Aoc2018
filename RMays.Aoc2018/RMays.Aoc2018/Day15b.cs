using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day15b
    {
        public class Grid
        {
            protected Spot[,] spotGrid { get; set; }
            public int Rows { get; set; }
            public int Cols { get; set; }
            public List<Unit> Units { get; set; }
            public int TicksCompleted { get; set; } = 0;
            public bool CombatDone =>
                TicksCompleted > 100000 ||
                Units.Where(x => x.IsAlive).Select(x => x.UnitType).Distinct().Count() <= 1;
            public int CombatScore => TicksCompleted * Units.Where(x => x.IsAlive).Sum(x => x.HP);

            public Grid(string input)
            {
                int numRows;
                int numCols;
                var charGrid = GetCharGridFromInput(input, out numRows, out numCols);
                Rows = numRows;
                Cols = numCols;

                // Using the char grid, initialize the spotGrid and Units.
                InitializeGrid(charGrid);
            }

            public void ProcessTick()
            {
                var unitIds = Units.OrderBy(x => x.Row).ThenBy(x => x.Col).Select(x => x.Id).ToList();

                foreach (var id in unitIds)
                {
                    ProcessUnit(Units.Where(x => x.Id == id).First());
                }

                TicksCompleted++;
            }

            private void ProcessUnit(Unit unit)
            {
                if (!unit.IsAlive) return;

                // Should we stay or should we move?
                var adjacentEnemyUnitId = AdjacentEnemyToAttack(unit);
                if (adjacentEnemyUnitId == -1)
                {
                    // Try to move.

                    // Are we within range now?
                    adjacentEnemyUnitId = AdjacentEnemyToAttack(unit);
                }

                if (adjacentEnemyUnitId > -1)
                {
                    // Attack!
                    Attack(unit.Id, adjacentEnemyUnitId);
                }
            }

            private void Attack(int sourceUnitId, int targetUnitId)
            {
                var sourceUnit = Units.Where(x => x.Id == sourceUnitId).First();
                var targetUnit = Units.Where(x => x.Id == targetUnitId).First();
                targetUnit.HP -= sourceUnit.AP;

                // Clear the spot if the unit died.
                if (!targetUnit.IsAlive)
                {
                    this.spotGrid[targetUnit.Row, targetUnit.Col] = Spot.Space;
                }
            }

            /// <summary>
            /// Is this unit next to an enemy?  If so, return the ID of the unit that it should attack.
            /// </summary>
            /// <param name="unit"></param>
            /// <returns></returns>
            private int AdjacentEnemyToAttack(Unit unit)
            {
                var lookingFor = (unit.UnitType == UnitType.Elf ? Spot.Goblin : Spot.Elf);
                // As a shortcut, assume the unit isn't on the edge.

                if (spotGrid[unit.Row - 1, unit.Col] == lookingFor)
                {
                    return Units.Where(x => x.IsAlive && x.Row == unit.Row - 1 && x.Col == unit.Col).First().Id;
                }
                if (spotGrid[unit.Row, unit.Col - 1] == lookingFor)
                {
                    return Units.Where(x => x.IsAlive && x.Row == unit.Row && x.Col == unit.Col - 1).First().Id;
                }
                if (spotGrid[unit.Row, unit.Col + 1] == lookingFor)
                {
                    return Units.Where(x => x.IsAlive && x.Row == unit.Row && x.Col == unit.Col + 1).First().Id;
                }
                if (spotGrid[unit.Row + 1, unit.Col] == lookingFor)
                {
                    return Units.Where(x => x.IsAlive && x.Row == unit.Row + 1 && x.Col == unit.Col).First().Id;
                }
                return -1;
            }


            #region Good so far

            private void InitializeGrid(char[,] charGrid)
            {
                spotGrid = new Spot[Rows, Cols];
                Units = new List<Unit>();

                for (var row = 0; row < Rows; row++)
                {
                    for (var col = 0; col < Cols; col++)
                    {
                        var spot = GetSpotFromChar(charGrid[row, col]);
                        spotGrid[row, col] = spot;
                        if (spot == Spot.Goblin || spot == Spot.Elf)
                        {
                            Units.Add(new Unit(spot) { Id = Units.Count(), Row = row, Col = col });
                        }
                    }
                }
            }

            private static Spot GetSpotFromChar(char spot)
            {
                switch(spot)
                {
                    case '#':
                        return Spot.Wall;
                    case '.':
                        return Spot.Space;
                    case 'E':
                        return Spot.Elf;
                    case 'G':
                        return Spot.Goblin;
                    default:
                        throw new ApplicationException($"Unexpected character: {spot}");
                }
            }

            private static char GetCharFromSpot(Spot spot)
            {
                switch (spot)
                {
                    case Spot.Wall:
                        return '#';
                    case Spot.Space:
                        return '.';
                    case Spot.Elf:
                        return 'E';
                    case Spot.Goblin:
                        return 'G';
                    default:
                        throw new ApplicationException($"Unexpected spot: {spot}");
                }
            }

            public override string ToString()
            {
                var toReturn = new StringBuilder();
                for (var row = 0; row < Rows; row++)
                {
                    for (var col = 0; col < Cols; col++)
                    {
                        toReturn.Append(GetCharFromSpot(spotGrid[row, col]));
                    }
                    toReturn.AppendLine();
                }
                return toReturn.ToString();
            }

            private char[,] GetCharGridFromInput(string input, out int numRows, out int numCols)
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

            #endregion
        }

        [Flags]
        public enum Spot
        {
            None = 0,

            Wall = 1,
            Space = 2,

            Elf = 4,
            Goblin = 8
        }

        public enum UnitType
        {
            Elf,
            Goblin
        }

        public class Unit
        {
            public UnitType UnitType { get; set; }
            public int Id { get; set; }
            public int Row { get; set; }
            public int Col { get; set; }
            public bool IsAlive => HP > 0;

            /// <summary>
            /// Remaining hit points
            /// </summary>
            public int HP { get; set; }

            /// <summary>
            /// Attack power
            /// </summary>
            public int AP { get; set; }

            public Unit() { }
            public Unit(UnitType unitType)
            {
                switch (unitType)
                {
                    case UnitType.Elf:
                        InitElf();
                        break;
                    case UnitType.Goblin:
                        InitGoblin();
                        break;
                }
            }

            public Unit(Spot spotType)
            {
                switch (spotType)
                {
                    case Spot.Elf:
                        InitElf();
                        break;
                    case Spot.Goblin:
                        InitGoblin();
                        break;
                }
            }

            private void InitElf()
            {
                this.UnitType = UnitType.Elf;
                this.HP = 200;
                this.AP = 3;
            }

            private void InitGoblin()
            {
                this.UnitType = UnitType.Goblin;
                this.HP = 200;
                this.AP = 3;
            }

            public override string ToString()
            {
                return $"{(UnitType == UnitType.Goblin ? "Gobin" : "Elf")} ({Row},{Col})";
            }
        }

        public long SolveA(string input)
        {
            var myGrid = new Grid(input);
            Console.WriteLine(myGrid);
            while (!myGrid.CombatDone)
            {
                myGrid.ProcessTick();
            }

            return myGrid.CombatScore;
        }

        public long SolveB(string input)
        {
            var myList = Parser.Tokenize(input);
      

            return 0;
        }
    }
}
