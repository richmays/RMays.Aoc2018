using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMays.Aoc2018.Tests
{
    [TestFixture]
    public class Day1_2019Tests
    {
        [Test]
        [TestCase("12", 2)]
        [TestCase("14", 2)]
        [TestCase("1969", 654)]
        [TestCase("100756", 33583)]
        public void PartATests(string input, int expectedOutput)
        {
            var day = new Day1_2019();
            var result = day.SolveA(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        [TestCase("14", 2)]
        [TestCase("1969", 966)]
        [TestCase("100756", 50346)]
        public void PartBTests(string input, int expectedOutput)
        {
            var day = new Day1_2019();
            var result = day.SolveB(input);
            Assert.AreEqual(expectedOutput, result);
        }

        [Test]
        public void DoItA() // ?
        {
            var day = new Day1_2019();
            Console.WriteLine(day.SolveA(InputData.Day1_2019));
        }

        [Test]
        public void DoItB() // ?
        {
            var day = new Day1_2019();
            Console.WriteLine(day.SolveB(InputData.Day1_2019));
        }
    }
}
