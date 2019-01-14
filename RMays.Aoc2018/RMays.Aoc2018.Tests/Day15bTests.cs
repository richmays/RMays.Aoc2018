using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day15bTests
    {
        [Test]
        [TestCase(@"######
#.GE.#
######", 134)]
        [TestCase(@"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######", 27730)]
        [TestCase(@"#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######", 36334)]
        [TestCase(@"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######", 39514)]
        [TestCase(@"#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######", 27755)]
        [TestCase(@"#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######", 28944)]
        [TestCase(@"#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########", 18740)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day15b();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("4, 5, 6", 456)]

        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day15b();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void DoItA() // ?
        {
            var day = new Day15b();
            Console.WriteLine(day.SolveA(InputData.Day15));
        }

        [Test]
        public void DoItB() // ?
        {
            var day = new Day15b();
            Console.WriteLine(day.SolveB(InputData.Day15));
        }

        [TestCase(1, 1, 2, 2, -1)]
        [TestCase(1, 2, 2, 2, -1)]
        [TestCase(0, 0, 2, 2, -1)]
        [TestCase(2, 2, 1, 1, 1)]
        [TestCase(2, 1, 1, 1, 1)]
        [TestCase(1, 2, 1, 1, 1)]
        [TestCase(1, 1, 1, 1, 0)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(1, 1, 1, 0, 1)]
        [TestCase(1, 1, 0, 1, 1)]
        [TestCase(1, 0, 1, 1, -1)]
        [TestCase(0, 1, 1, 1, -1)]
        public void CoordsTests(int row1, int col1, int row2, int col2, int expected)
        {
            /*
            int x = 0;
            int y = 1;
            Assert.AreEqual(-1, x.CompareTo(y));
            Assert.AreEqual(1, y.CompareTo(x));
            Assert.AreEqual(0, x.CompareTo(x));
            Assert.AreEqual(0, y.CompareTo(y));
            */

            var coords1 = new Coords { Row = row1, Col = col1 };
            var coords2 = new Coords { Row = row2, Col = col2 };
            Assert.AreEqual(expected, coords1.CompareTo(coords2));
        }

    }
}
