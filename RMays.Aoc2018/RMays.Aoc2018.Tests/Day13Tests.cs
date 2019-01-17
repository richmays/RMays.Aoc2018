﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day13Tests
    {
        [Test]
        [TestCase(@"/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   ", "7,3")]
        public void PartATests(string input, string expectedOutput)
        {
            var day = new Day13();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("4, 5, 6", 456)]

        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day13();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void DoItA() // 116,10
        {
            var day = new Day13();
            Console.WriteLine(day.SolveA(InputData.Day13));
        }

        [Test]
        public void DoItB() // 116,25 (after 143ms, first try.  wow.)
        {
            var day = new Day13();
            Console.WriteLine(day.SolveB(InputData.Day13));
        }
    }
}