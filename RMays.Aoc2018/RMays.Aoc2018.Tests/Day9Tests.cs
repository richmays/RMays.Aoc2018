using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day9Tests
    {
        [Test]
        [TestCase("5 players; last marble is worth 25 points", 32)]
        [TestCase("10 players; last marble is worth 1618 points", 8317)]
        [TestCase("13 players; last marble is worth 7999 points", 146373)]
        [TestCase("17 players; last marble is worth 1104 points", 2764)]
        [TestCase("21 players; last marble is worth 6111 points", 54718)]
        [TestCase("30 players; last marble is worth 5807 points", 37305)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day9();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("4, 5, 6", 456)]

        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day9();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void DoItA() // ?
        {
            var day = new Day9();
            Console.WriteLine(day.SolveA(InputData.Day9));
        }

        [Test]
        public void DoItB() // ?
        {
            var day = new Day9();
            Console.WriteLine(day.SolveB(InputData.Day9));
        }
    }
}
