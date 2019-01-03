using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day12Tests
    {
        [Test]
        [TestCase(@"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #", 325)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day12();
            var result = day.SolveA(input, 20);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void PartATestsRealData()
        {
            var expectedOutput = 4386;
            var day = new Day12();
            var result = day.SolveA(InputData.Day12, 20);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void DoItA() // 4386
        {
            var day = new Day12();
            Console.WriteLine(day.SolveA(InputData.Day12, 20));
        }

        [Test]
        public void DoItB() // ?
        {
            var day = new Day12();
            Console.WriteLine(day.SolveA(InputData.Day12, 5000));
            //Console.WriteLine(day.SolveA(InputData.Day12, 50000000000));
        }
    }
}
