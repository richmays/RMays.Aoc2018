using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day20Tests
    {
        [Test]
        [TestCase("^WNE$", 3)]
        [TestCase("^ENWWW(NEEE|SSE(EE|N))$", 10)]
        [TestCase("^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$", 18)]
        [TestCase("^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$", 23)]
        [TestCase("^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$", 31)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day20();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        /*
        [Test]
        [TestCase("4, 5, 6", 456)]
        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day20();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }
        */

        [Test]
        public void DoItA() // ?
        {
            var day = new Day20();
            Console.WriteLine(day.SolveA(InputData.Day20));
        }

        public void DoItB() // ?
        {
            var day = new Day20();
            Console.WriteLine(day.SolveB(InputData.Day20));
        }
    }
}
