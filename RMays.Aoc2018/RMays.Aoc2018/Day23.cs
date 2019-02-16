using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018
{
    public class Day23
    {
        public class Star : IEquatable<Star>
        {
            public long X { get; set; }
            public long Y { get; set; }
            public long Z { get; set; }
            public long Range { get; set; }

            public Star()
            {
            }

            public Star(long x, long y, long z, long range)
            {
                X = x;
                Y = y;
                Z = z;
                Range = range;
            }

            public bool IsWithinRange(Star star)
            {
                var dist = Math.Abs(X - star.X) + Math.Abs(Y - star.Y) + Math.Abs(Z - star.Z);
                return dist <= Range;
            }

            public long ManhattanFromZero()
            {
                return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            }

            public override string ToString()
            {
                return $"({X},{Y},{Z}) r={Range}";
            }

            public bool Equals(Star other)
            {
                return (this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.Range == other.Range);
            }
        }

        public List<Star> GetStars(string input)
        {
            // pos=<29104291,6406472,65924134>, r=65916701
            // pos=<-2507185,84321079,68857085>, r=62423035

            var lines = Parser.TokenizeLines(input);
            var Stars = new List<Star>();
            foreach (var line in lines)
            {
                var x = long.Parse(line.Split('<')[1].Split(',')[0]);
                var y = long.Parse(line.Split(',')[1]);
                var z = long.Parse(line.Split(',')[2].Split('>')[0]);
                var range = long.Parse(line.Split('=')[2]);
                Stars.Add(new Star(x, y, z, range));
            }

            return Stars;
        }

        public long SolveA(string input)
        {
            var Stars = GetStars(input);

            var bestStar = Stars.OrderByDescending(x => x.Range).First();
            int inRange = 0;

            foreach(var star in Stars)
            {
                if (bestStar.IsWithinRange(star)) inRange++;
            }

            return inRange;
        }

        public long SolveB(string input)
        {
            var Stars = GetStars(input);

            // bool: false for floor, true for ceiling
            var StarTuples = new List<Tuple<long, bool>>();

            foreach(var star in Stars)
            {
                var floor = star.ManhattanFromZero() - star.Range;
                var ceiling = star.ManhattanFromZero() + star.Range;
                StarTuples.Add(new Tuple<long, bool>(floor, false));
                StarTuples.Add(new Tuple<long, bool>(ceiling, true));
            }

            StarTuples = StarTuples.OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList();

            var bestCount = 0;
            var currCount = 0;
            var bestResult = (long)-1;
            foreach(var tuple in StarTuples)
            {
                currCount += (tuple.Item2 ? -1 : 1);
                if (currCount > bestCount)
                {
                    bestCount = currCount;
                    bestResult = tuple.Item1;
                }
            }

            return bestResult;
        }
    }
}
