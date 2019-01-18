using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day17Tests
    {
        [Test]
        [TestCase(@"x=495, y=2..7
y=7, x=495..501
x=501, y=3..7
x=498, y=2..4
x=506, y=1..2
x=498, y=10..13
x=504, y=10..13
y=13, x=498..504", 57)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day17();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("4, 5, 6", 456)]

        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day17();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void DoItA() // ?
        {
            var day = new Day17();
            Console.WriteLine(day.SolveA(InputData.Day17));
        }

        [Test]
        public void DoItB() // ?
        {
            var day = new Day17();
            Console.WriteLine(day.SolveB(InputData.Day17));
        }
    }
}
