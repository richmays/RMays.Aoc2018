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
                TicksCompleted > 1 ||
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
                    // (Moving will never kill a unit.  There's no poison ground, etc.  So we don't need to check for the unit's death after it moves.)
                    MoveUnit(unit.Id);

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
            /// Move the unit to wherever it should go.
            /// </summary>
            /// <param name="unitId"></param>
            private void MoveUnit(int unitId)
            {
                // Process:
                // 1. Get all coordinates that are empty squares adjacent to an enemy.
                // 2. From those, select only the coordinates that can be reached by the current unit.
                // 3. From those, select the nearest coordinates.
                // 4. From those, sort by the path (prefer lowest row, then lowest col).
                // After we've chosen a target coordinate,
                // 5. Take the step that is first in reading order that would get it the closest.

                var coordTargets = new List<Coords>();
                var unit = Units.First(x => x.Id == unitId);
                var enemyType = unit.UnitType == UnitType.Elf ? UnitType.Goblin : UnitType.Elf;

                // 1
                foreach (var enemyUnit in Units.Where(x => x.IsAlive && x.UnitType == enemyType).ToList())
                {
                    if (spotGrid[enemyUnit.Row - 1, enemyUnit.Col] == Spot.Space)
                    {
                        coordTargets.Add(new Coords { Row = enemyUnit.Row - 1, Col = enemyUnit.Col });
                    }
                    if (spotGrid[enemyUnit.Row, enemyUnit.Col - 1] == Spot.Space)
                    {
                        coordTargets.Add(new Coords { Row = enemyUnit.Row, Col = enemyUnit.Col - 1 });
                    }
                    if (spotGrid[enemyUnit.Row, enemyUnit.Col + 1] == Spot.Space)
                    {
                        coordTargets.Add(new Coords { Row = enemyUnit.Row, Col = enemyUnit.Col + 1 });
                    }
                    if (spotGrid[enemyUnit.Row + 1, enemyUnit.Col] == Spot.Space)
                    {
                        coordTargets.Add(new Coords { Row = enemyUnit.Row + 1, Col = enemyUnit.Col });
                    }
                }
                coordTargets = coordTargets.Distinct().ToList();

                // 2
                var allReachableSpots = GetReachableSpots(new Coords { Row = unit.Row, Col = unit.Col });
                var reachableSpots = allReachableSpots.Keys
                    .Where(x => coordTargets.Select(y => y.ToString()).Contains(x.ToString()))
                    .ToList();




                /*
                int steps;
                if (IsReachable(new Coords { Row = unit.Row, Col = unit.Col }, coordTargets[0], out steps))
                {
                    // Great!  We can reach it in 'steps' steps.
                }
                */
            }

            private Dictionary<Coords, PathData> GetReachableSpots(Coords start)
            {
                var toReturn = new Dictionary<Coords, PathData>();

                // Let's not worry about optimization yet.  The simple solution might be good enough.
                var reached = new Dictionary<string, PathData>();

                var pathData = new PathData();
                reached.Add(start.ToString(), pathData);
                pathData.Push(Direction.Up);
                GetReachableSpotsRecursive(start.Up(), (PathData)pathData.Clone(), reached);
                pathData.Pop();
                pathData.Push(Direction.Left);
                GetReachableSpotsRecursive(start.Left(), (PathData)pathData.Clone(), reached);
                pathData.Pop();
                pathData.Push(Direction.Right);
                GetReachableSpotsRecursive(start.Right(), (PathData)pathData.Clone(), reached);
                pathData.Pop();
                pathData.Push(Direction.Down);
                GetReachableSpotsRecursive(start.Down(), (PathData)pathData.Clone(), reached);
                pathData.Pop();

                foreach (var key in reached.Keys)
                {
                    var keySplit = key.Split(',');
                    toReturn.Add(new Coords { Row = int.Parse(keySplit[0]), Col = int.Parse(keySplit[1]) }, reached[key]);
                }

                return toReturn;
            }

            private void GetReachableSpotsRecursive(Coords start, PathData pathSoFar, Dictionary<string, PathData> reached)
            {
                // If this is already in the dictionary, jump out.
                if (reached.Keys.Contains(start.ToString()))
                {
                    if (reached[start.ToString()].CompareTo(pathSoFar) < 0)
                    {
                        return;
                    }
                    else
                    {
                        reached.Remove(start.ToString());
                    }
                }

                reached.Add(start.ToString(), (PathData)pathSoFar.Clone());

                // If we're not on a space, jump out.
                if (GetSpot(start) != Spot.Space)
                {
                    return;
                }

                pathSoFar.Push(Direction.Up);
                GetReachableSpotsRecursive(start.Up(), (PathData)pathSoFar.Clone(), reached);
                pathSoFar.Pop();
                pathSoFar.Push(Direction.Left);
                GetReachableSpotsRecursive(start.Left(), (PathData)pathSoFar.Clone(), reached);
                pathSoFar.Pop();
                pathSoFar.Push(Direction.Right);
                GetReachableSpotsRecursive(start.Right(), (PathData)pathSoFar.Clone(), reached);
                pathSoFar.Pop();
                pathSoFar.Push(Direction.Down);
                GetReachableSpotsRecursive(start.Down(), (PathData)pathSoFar.Clone(), reached);
                pathSoFar.Pop();
            }

            /*
            /// <summary>
            /// How many steps will it take to reach the target?
            /// Stop counting at a non-space.
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            private bool IsReachable(Coords start, Coords end, out int steps)
            {
                // Base case check.
                if (start == end)
                {
                    steps = 0;
                    return true;
                }

                // Let's not worry about optimization yet.  The simple solution might be good enough.
                var reached = new Dictionary<string, int>();
                reached.Add(start.ToString(), 0);
                IsReachableRecursive(start.Up(), end, 1, reached);
                IsReachableRecursive(start.Left(), end, 1, reached);
                IsReachableRecursive(start.Right(), end, 1, reached);
                IsReachableRecursive(start.Down(), end, 1, reached);

                steps = (reached.Keys.Contains(end.ToString()) ? reached[end.ToString()] : -1);
                return reached.Keys.Contains(end.ToString());
            }

            private void IsReachableRecursive(Coords start, Coords end, int stepsSoFar, Dictionary<string, int> reached)
            {
                // If this is already in the dictionary, jump out.
                if (reached.Keys.Contains(start.ToString()))
                {
                    if (reached[start.ToString()] <= stepsSoFar)
                    {
                        return;
                    }
                    else
                    {
                        reached.Remove(start.ToString());
                    }
                }

                reached.Add(start.ToString(), stepsSoFar);

                // If we reached the end, jump out.  (No need to build up the rest of the dictionary.)
                if (start == end)
                {
                    return;
                }

                // If we're not on a space, jump out.
                if (GetSpot(start) != Spot.Space)
                {
                    return;
                }

                if (!reached.Keys.Contains(start.Up().ToString()))
                {
                    IsReachableRecursive(start.Up(), end, stepsSoFar + 1, reached);
                }
                if (!reached.Keys.Contains(start.Left().ToString()))
                {
                    IsReachableRecursive(start.Left(), end, stepsSoFar + 1, reached);
                }
                if (!reached.Keys.Contains(start.Right().ToString()))
                {
                    IsReachableRecursive(start.Right(), end, stepsSoFar + 1, reached);
                }
                if (!reached.Keys.Contains(start.Down().ToString()))
                {
                    IsReachableRecursive(start.Down(), end, stepsSoFar + 1, reached);
                }
            }
            */

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

            private Spot GetSpot(Coords coords)
            {
                return this.spotGrid[coords.Row, coords.Col];
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

        public class PathData : IComparable<PathData>, IEquatable<PathData>, ICloneable
        {
            public int Steps => Directions.Count();
            public List<Direction> Directions { get; set; }

            public PathData()
            {
                Directions = new List<Direction>();
            }

            public void Push(Direction dir)
            {
                Directions.Add(dir);
            }

            public Direction Pop()
            {
                var toReturn = Directions.Last();
                Directions.RemoveAt(Directions.Count - 1);
                return toReturn;
            }

            public Direction PopFirst()
            {
                var toReturn = Directions.First();
                Directions.RemoveAt(0);
                return toReturn;
            }

            public int CompareTo(PathData other)
            {
                if (this.Steps < other.Steps) return -1;
                if (this.Steps > other.Steps) return 1;
                for (int i = 0; i < this.Steps; i++)
                {
                    if (this.Directions[i] < other.Directions[i]) return -1;
                    if (this.Directions[i] > other.Directions[i]) return 1;
                }
                return 0;
            }

            public bool Equals(PathData other)
            {
                return (this.CompareTo(other) == 0);
            }

            public object Clone()
            {
                return new PathData { Directions = this.Directions.ToList() };
            }

            public override string ToString()
            {
                return $"{Steps}: {string.Join("", this.Directions.Select(x => x.ToString()[0]).ToArray())}";
            }
        }

        public enum Direction
        {
            Up = 0,
            Left = 1,
            Right = 2,
            Down = 3
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
