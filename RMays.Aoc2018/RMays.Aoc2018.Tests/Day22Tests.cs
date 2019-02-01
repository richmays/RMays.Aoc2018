using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day22Tests
    {
        [Test]
        [TestCase(@"depth: 510
target: 10,10", 114)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day22();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        /*
        [Test]
        [TestCase("4, 5, 6", 456)]

        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day22();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }
        */

        [Test]
        public void DoItA() // 11575, not too tricky
        {
            var day = new Day22();
            Console.WriteLine(day.SolveA(InputData.Day22));
        }

        public void DoItB() // ?
        {
            var day = new Day22();
            Console.WriteLine(day.SolveB(InputData.Day22));
        }
    }
}
